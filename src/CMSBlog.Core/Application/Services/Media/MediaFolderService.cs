using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMSBlog.Core.Application.Interfaces;
using CMSBlog.Core.Application.Interfaces.Media;
using CMSBlog.Core.Domain.Media;
using CMSBlog.Core.Application.DTOs.Media;


namespace CMSBlog.Core.Application.Services.Media
{
    internal class MediaFolderService : IMediaFolderService
    {
        private readonly IStorageServicee _storage;
        private readonly IRepository<MediaFolder> _repo;
        private readonly IMapper _mapper;

        public MediaFolderService(IRepository<MediaFolder> repo, IStorageServicee storage, IMapper mapper)
        {
            _repo = repo;
            _storage = storage;
            _mapper = mapper;
        }

        public async Task<MediaFolderDto> CreateAsync(CreateMediaFolderDto dto)
        {
            var slug = GenerateSlug(dto.FolderName);

            // Map DTO → Entity
            var entity = _mapper.Map<MediaFolder>(dto);
            entity.ID = Guid.NewGuid();
            entity.SlugName = slug;
            entity.DateCreated = DateTime.UtcNow;


            await _repo.AddAsync(entity);

            // Lưu thư mục vật lý (Strapi-like)
           // await _storage.CreateFolderAsync(entity.SlugName, entity.ParentFolderId);


            // Map Entity → DTO
            var folderDto = _mapper.Map<MediaFolderDto>(entity);
            return folderDto;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return false;
            var result = await _repo.DeleteAsync(entity);
            return result;
        }

        public async Task<List<MediaFolderDto>> GetAllAsync()
        {
            var items =  await _repo.GetAllAsync();
            var result = _mapper.Map<List<MediaFolderDto>>(items);
            return result;
        }

        public async Task<MediaFolderDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return null;
            var folderDto = _mapper.Map<MediaFolderDto>(entity);
            return folderDto;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateMediaFolderDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) 
                return false;
            // Map DTO → Entity
            _mapper.Map(dto, entity);
            await _repo.SaveChangesAsync();
            return true;
        }

        // Hàm tạo Slug từ tên thư mục
        private string GenerateSlug(string name)
        {
            return name
                .Trim()
                .ToLower()
                .Replace(" ", "-")
                .Replace("_", "-");
        }
    }
}
