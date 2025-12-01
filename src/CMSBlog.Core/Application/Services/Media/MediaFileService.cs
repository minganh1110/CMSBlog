using AutoMapper;
using CMSBlog.Core.Application.DTOs.Media;
using CMSBlog.Core.Application.Interfaces.Media;
using CMSBlog.Core.Domain.Media;
using CMSBlog.Core.Domain.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CMSBlog.Core.Application.Services.Media
{
    public class MediaFileService : IMediaFileService
    {
        private readonly IMediaFileRepository _repo;
        private readonly IMapper _mapper;
        private readonly IStorageFactory _storageFactory;
        private readonly IMediaFolderRepository _folderRepo;
        private readonly IFileFolderLinkRepository _fileFolderRepo;

        public async Task<string> UploadAsync(byte[] content, string fileName)
        {
            var provider = await _storageFactory.GetStorageServiceAsync();
            return await provider.SaveFileAsync(content, fileName);
        }

        public MediaFileService(IMediaFileRepository repo, IMapper mapper, IStorageFactory storageFactory, 
            IMediaFolderRepository folderrepo, IFileFolderLinkRepository linkrepo)
        {
            _repo = repo;
            _mapper = mapper; 
            _storageFactory = storageFactory;
            _folderRepo = folderrepo;
            _fileFolderRepo = linkrepo;
        }

        public async Task<MediaFileDto> UploadAsync(CreatedMediaFileDto dto, CancellationToken ct = default)
        {
            if (dto.FileContent is null || dto.FileContent.Length == 0)
                throw new ArgumentException("File is empty");

            // 1. Slug + file name
            var slug = GenerateSlug(Path.GetFileNameWithoutExtension(dto.FileName));
            var ext = Path.GetExtension(dto.FileName);
            var fileName = $"{slug}{ext}";

            // 2. Lưu file qua provider
            var storage = await _storageFactory.GetStorageServiceAsync();
            var storagePath = await storage.SaveFileAsync(dto.FileContent, fileName, ct);

            // 3. Xác định folderId (nếu null thì gán ROOT)
            Guid folderId = dto.FolderId ?? MediaFolderConstants.RootFolderId;

            // Kiểm tra folder có tồn tại không
            var folder = await _folderRepo.GetByIdAsync(folderId)
                        ?? throw new Exception("Folder does not exist");

            // 4. Tạo MediaFile
            var entity = new MediaFile
            {
                ID = Guid.NewGuid(),
                FileName = dto.FileName,
                SlugName = slug,
                FileExtension = ext,
                FilePath = storagePath,
                FileSize = dto.FileContent.Length,
                MimeType = dto.MimeType,
                DateCreated = DateTime.UtcNow,
                Provider = storage.ProviderName,
                MediaType = dto.MediaType
            };

            await _repo.AddAsync(entity);

            // 5. Tạo liên kết qua bảng trung gian MediaFileFolder
            await _fileFolderRepo.AddLinkAsync(entity.ID, folderId);

            // 6. Trả về DTO
            var result = _mapper.Map<MediaFileDto>(entity);
            result.FileUrl = $"{storage.GetPublicBaseUrl()}/{storagePath}";
            result.FolderId = folderId;

            return result;
        }


        public async Task<IEnumerable<MediaFileDto>> GetAllAsync()
        {
            var storage = await _storageFactory.GetStorageServiceAsync();
            var providerName = storage.ProviderName;

            var items = await _repo.GetAllAsync(providerName);

            var baseUrl = storage.GetPublicBaseUrl();
            var result = _mapper.Map<List<MediaFileDto>>(items);

            foreach (var file in result)
                file.FileUrl = $"{baseUrl}/{file.FilePath}";

            return result;
        }


        public async Task<List<MediaFileDto>> GetInFolderAsync(Guid folderId)
        {
            var storage = await _storageFactory.GetStorageServiceAsync();
            var providerName = storage.ProviderName;

            var items = await _repo.GetByFolderIdAsync(folderId, providerName);

            var baseUrl = storage.GetPublicBaseUrl();
            var result = _mapper.Map<List<MediaFileDto>>(items);

            foreach (var file in result)
                file.FileUrl = $"{baseUrl}/{file.FilePath}";

            return result;
        }


        public async Task<bool> UpdateAsync(Guid id, UpdateMediaFileDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            // Map DTO → entity (partial update)
            _mapper.Map(dto, entity);

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<MediaFileDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            var storage = await _storageFactory.GetStorageServiceAsync();
            var baseUrl = storage.GetPublicBaseUrl();
            var dto = _mapper.Map<MediaFileDto>(entity);
            dto.FileUrl = $"{baseUrl}/{dto.FilePath}";
            return dto;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
            return true;
        }

        private string GenerateSlug(string input)
        {
            return input.ToLowerInvariant().Replace(" ", "-");
        }

        public MediaType DetectMediaType(string mime)
        {
            if (string.IsNullOrWhiteSpace(mime))
                return MediaType.Other;

            if (mime.StartsWith("image/"))
                return MediaType.Image;

            if (mime.StartsWith("video/"))
                return MediaType.Video;

            if (mime.StartsWith("audio/"))
                return MediaType.Audio;

            if (mime == "application/pdf")
                return MediaType.Document;

            if (mime.Contains("excel") || mime.Contains("spreadsheet"))
                return MediaType.Document;

            if (mime.Contains("word"))
                return MediaType.Document;

            return MediaType.Other;
        }

    }
}
