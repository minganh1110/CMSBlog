using AutoMapper;
using CMSBlog.Core.Application.DTOs.Media;
using CMSBlog.Core.Application.Interfaces.Media;
using CMSBlog.Core.Domain.Constants;
using CMSBlog.Core.Domain.Media;

namespace CMSBlog.Core.Application.Services.Media
{
    public class MediaFolderService : IMediaFolderService
    {
        private readonly IMediaFolderRepository _repo;
        private readonly IFileFolderLinkRepository _linkRepo;
        private readonly IMapper _mapper;
        private readonly IStorageFactory _storageFactory;

        public MediaFolderService(
            IMediaFolderRepository repo,
            IFileFolderLinkRepository linkRepo,
            IMapper mapper,
            IStorageFactory storageFactory)
        {
            _repo = repo;
            _linkRepo = linkRepo;
            _mapper = mapper;
            _storageFactory = storageFactory;
        }

        // -----------------------------------------------------------
        // CREATE
        // -----------------------------------------------------------
        public async Task<MediaFolderDto> CreateAsync(CreateMediaFolderDto dto, Guid? userId = null)
        {
            // 3. Xác định folderId (nếu null thì gán ROOT)
            Guid folderId = dto.ParentFolderId ?? MediaFolderConstants.RootFolderId;
            var entity = new MediaFolder()
            {
                Id = Guid.NewGuid(),
                FolderName = dto.FolderName,
                SlugName = GenerateSlug(dto.FolderName),
                ParentFolderId = folderId,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            // Build Path
            if (dto.ParentFolderId == null)
            {
                entity.Path = "/" + entity.PathId;
            }
            else
            {
                var parent = await _repo.GetByIdAsync(dto.ParentFolderId.Value)
                             ?? throw new InvalidOperationException("Parent folder not found");

                entity.Path = $"{parent.Path}/{entity.PathId}";
            }

            // Chỉ update field Path, không update cả object
            _repo.Attach(entity);
            _repo.SetPropertyModified(entity, x => x.Path);
            await _repo.SaveChangesAsync();

            return _mapper.Map<MediaFolderDto>(entity);
        }

        // -----------------------------------------------------------
        // GET BY ID
        // -----------------------------------------------------------
        public async Task<MediaFolderDto?> GetByIdAsync(Guid id)
        {
            var all = await _repo.GetAllAsync();  // 🚀 lấy tất cả folders 1 lần
            var storage = await _storageFactory.GetStorageServiceAsync();
            var providerName = storage.ProviderName;

            var fileCounts = await _repo.GetFileCountsAsync(providerName);

            // Dùng dictionary
            var map = all.ToDictionary(x => x.Id, x => _mapper.Map<MediaFolderDto>(x));

            // Set count
            foreach (var item in map.Values)
                item.FileCount = fileCounts.ContainsKey(item.Id) ? fileCounts[item.Id] : 0;

            // Tạo rỗng children
            foreach (var item in map.Values)
                item.Children = new List<MediaFolderDto>();

            // Build cây
            foreach (var f in all)
            {
                if (f.ParentFolderId.HasValue)
                    map[f.ParentFolderId.Value].Children.Add(map[f.Id]);
            }

            // Trả lại đúng folder user đang cần
            return map[id];
        }




        //
        // GET BY ID INCLUDE FILES
        //
        public async Task<MediaFolderDto?> GetByIdIncludeFilesAsync(Guid id)
        {
            var storage = await _storageFactory.GetStorageServiceAsync();
            var providerName = storage.ProviderName;

            var entity = await _repo.GetByIdWithChildrenAsync(id, providerName);
            return entity == null ? null : _mapper.Map<MediaFolderDto>(entity);
        }

        // -----------------------------------------------------------
        // GET TREE
        // -----------------------------------------------------------
        public async Task<List<MediaFolderDto>> GetTreeAsync()
        {
            var all = await _repo.GetAllAsync();

            var storage = await _storageFactory.GetStorageServiceAsync();
            var providerName = storage.ProviderName;

            // 👇 Lấy số lượng file theo folderId
            var fileCounts = await _repo.GetFileCountsAsync(providerName);

            // Map DTO
            var dtoMap = all.ToDictionary(
                x => x.Id,
                x =>
                {
                    var dto = _mapper.Map<MediaFolderDto>(x);
                    dto.Children = new List<MediaFolderDto>();

                    // 👇 Gán số lượng file
                    dto.FileCount = fileCounts.ContainsKey(x.Id)
                        ? fileCounts[x.Id]
                        : 0;

                    return dto;
                });

            var roots = new List<MediaFolderDto>();

            foreach (var f in all)
            {
                if (f.ParentFolderId.HasValue)
                {
                    dtoMap[f.ParentFolderId.Value].Children!.Add(dtoMap[f.Id]);

                }
                else
                {
                    roots.Add(dtoMap[f.Id]);
                }
            }

            return roots;
        }

        // -----------------------------------------------------------
        // RENAME
        // -----------------------------------------------------------
        public async Task<bool> RenameAsync(Guid id, string newName)
        {
            var folder = await _repo.GetByIdAsync(id);
            if (folder == null) return false;

            folder.FolderName = newName;
            folder.SlugName = GenerateSlug(newName);
            folder.DateModified = DateTime.UtcNow;

            await _repo.UpdateAsync(folder);
            await _repo.SaveChangesAsync();

            return true;
        }

        // -----------------------------------------------------------
        // Edit
        // -----------------------------------------------------------
        public async Task<bool> EditAsync(Guid id, UpdateMediaFolderDto dto)
        {
            var folder = await _repo.GetByIdAsync(id);
            if (folder == null) return false;

            await _repo.BeginTransactionAsync();
            try
            {
                bool isRenamed = false;
                bool isMoved = false;

                if (!string.IsNullOrWhiteSpace(dto.FolderName)
                && dto.FolderName != folder.FolderName)
                {
                    
                    isRenamed = await RenameAsync(id, dto.FolderName);
                }

                if (dto.NewFolderId != folder.ParentFolderId)
                {
                    
                    isMoved = await MoveAsync(id, dto.NewFolderId);
                }
            }
            catch
            {
                await _repo.RollbackAsync();
                throw;
            }
       
            return true;
        }

        // -----------------------------------------------------------
        // MOVE
        // -----------------------------------------------------------
        public async Task<bool> MoveAsync(Guid id, Guid? newParentId)
        {
            var folder = await _repo.GetByIdAsync(id);
            if (folder == null) return false;

            string oldPath = folder.Path;
            string newPath;

            // Determine new path
            if (newParentId is Guid parentId)
            {
                var parent = await _repo.GetByIdAsync(parentId)
                             ?? throw new InvalidOperationException("New parent not found");

                // Prevent move into its own children
                if (parent.Path.StartsWith(folder.Path))
                    throw new InvalidOperationException("Cannot move folder into its descendant");

                folder.ParentFolderId = parentId;
                newPath = $"{parent.Path}/{folder.PathId}";
            }
            else
            {
                folder.ParentFolderId = null;
                newPath = $"/{folder.PathId}";
            }

            // Transaction handled by repository
            await _repo.BeginTransactionAsync();

            try
            {
                folder.Path = newPath;
                folder.DateModified = DateTime.UtcNow;
                await _repo.UpdateAsync(folder);

                // Update descendants path
                var descendants = await _repo.GetDescendantsAsync(oldPath);
                foreach (var d in descendants)
                {
                    var suffix = d.Path.Substring(oldPath.Length);
                    d.Path = newPath + suffix;
                    d.DateModified = DateTime.UtcNow;

                    await _repo.UpdateAsync(d);
                }

                //await _repo.SaveChangesAsync();
                await _repo.CommitAsync();

                return true;
            }
            catch
            {
                await _repo.RollbackAsync();
                throw;
            }
        }

        // -----------------------------------------------------------
        // DELETE
        // -----------------------------------------------------------
        public async Task<bool> DeleteAsync(Guid id, bool removeFileLinks = true)
        {
            var folder = await _repo.GetByIdAsync(id);
            if (folder == null) return false;

            var pathPrefix = folder.Path;

            var descendants = await _repo.GetDescendantsAsync(pathPrefix);
            var allToDelete = descendants.Append(folder).ToList();

            await _repo.BeginTransactionAsync();

            try
            {
                if (removeFileLinks)
                {
                    var ids = allToDelete.Select(f => f.Id).ToList();
                    await _linkRepo.DeleteByFolderIdsAsync(ids);
                }

                await _repo.DeleteRangeAsync(allToDelete);
                await _repo.SaveChangesAsync();

                await _repo.CommitAsync();
                return true;
            }
            catch
            {
                await _repo.RollbackAsync();
                throw;
            }
        }

        // -----------------------------------------------------------
        // FILTER
        // -----------------------------------------------------------
        public async Task<List<MediaFolderDto>> FilterAsync(FilterFolderDto f)
        {
            var query = (await _repo.GetAllAsync()).AsQueryable();

            // Parent filter
            if (f.ParentId.HasValue)
                query = query.Where(x => x.ParentFolderId == f.ParentId.Value);

            // Search by name
            if (!string.IsNullOrWhiteSpace(f.Search))
            {
                var keyword = f.Search.ToLower();
                query = query.Where(x => x.FolderName.ToLower().Contains(keyword));
            }

            // Date range
            if (f.CreatedFrom.HasValue)
                query = query.Where(x => x.DateCreated >= f.CreatedFrom.Value);

            if (f.CreatedTo.HasValue)
                query = query.Where(x => x.DateCreated <= f.CreatedTo.Value);

            if (f.ModifiedFrom.HasValue)
                query = query.Where(x => x.DateModified >= f.ModifiedFrom.Value);

            if (f.ModifiedTo.HasValue)
                query = query.Where(x => x.DateModified <= f.ModifiedTo.Value);

            // Sort
            query = f.SortBy?.ToLower() switch
            {
                "created" => (f.SortDir == "desc" ? query.OrderByDescending(x => x.DateCreated)
                                                   : query.OrderBy(x => x.DateCreated)),
                "modified" => (f.SortDir == "desc" ? query.OrderByDescending(x => x.DateModified)
                                                   : query.OrderBy(x => x.DateModified)),
                "path" => (f.SortDir == "desc" ? query.OrderByDescending(x => x.Path)
                                                   : query.OrderBy(x => x.Path)),
                _ => (f.SortDir == "desc" ? query.OrderByDescending(x => x.FolderName)
                                          : query.OrderBy(x => x.FolderName))
            };

            return query.Select(x => _mapper.Map<MediaFolderDto>(x)).ToList();
            
        }

        // -----------------------------------------------------------
        // HELPERS
        // -----------------------------------------------------------
        private string GenerateSlug(string input)
            => input.Trim().ToLower().Replace(" ", "-");

        private MediaFolderDto MapFolderTree(
        MediaFolder entity,
        Dictionary<Guid, int> fileCounts)
            {
                var dto = _mapper.Map<MediaFolderDto>(entity);

                dto.FileCount = fileCounts.TryGetValue(entity.Id, out var count)
                    ? count
                    : 0;

                dto.Children = new List<MediaFolderDto>();

                if (entity.ChildFolders == null || !entity.ChildFolders.Any())
                    return dto;

                foreach (var child in entity.ChildFolders)
                {
                    dto.Children.Add(MapFolderTree(child, fileCounts));
                }

                return dto;
            }



    }
}
