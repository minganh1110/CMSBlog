using AutoMapper;
using CMSBlog.Core.Application.DTOs.Media;
using CMSBlog.Core.Application.Interfaces.Media;
using CMSBlog.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CMSBlog.Core.Application.Services.Media
{
    public class MediaFileService : IMediaFileService
    {
        private readonly IMediaFileRepository _repo;
        private readonly IStorageServicee _storage;
        private readonly IMapper _mapper;

        public MediaFileService(IMediaFileRepository repo, IStorageServicee storage, IMapper mapper)
        {
            _repo = repo;
            _storage = storage;
            _mapper = mapper;
        }

        public async Task<MediaFileDto> UploadAsync(CreatedMediaFileDto dto, CancellationToken ct = default)
        {
            if (dto.FileContent.Length == 0)
                throw new ArgumentException("File is empty");

            // Tạo slug + đặt lại tên file
            var slug = GenerateSlug(Path.GetFileNameWithoutExtension(dto.FileName));
            var ext = Path.GetExtension(dto.FileName);
            var fileName = $"{slug}{ext}";

            // Lưu file thông qua interface
            var storagePath = await _storage.SaveFileAsync(dto.FileContent, fileName, ct);

            // Map DTO → Entity
            var entity = _mapper.Map<MediaFile>(dto);
            //entity.ID = Guid.NewGuid();
            //entity.FilePath = storagePath;
            //entity.SlugName = slug;
            //entity.FileExtension = ext;
            //entity.FileSize = dto.FileContent.Length;
            //entity.DateCreated = DateTime.UtcNow;

            //await _repo.AddAsync(entity);

            //var entity = new MediaFile
            //{
            entity.ID = Guid.NewGuid();
            entity.FileName = dto.FileName;
            entity.SlugName = slug;
            entity.FilePath = storagePath;
            entity.FileExtension = ext;
            entity.FileSize = dto.FileContent.Length;
            entity.FolderId = dto.FolderId;
            entity.DateCreated = DateTime.UtcNow;
            //};

            await _repo.AddAsync(entity);

            // Map Entity → DTO
            var result = _mapper.Map<MediaFileDto>(entity);
            result.FileUrl = $"{_storage.GetPublicBaseUrl()}/{storagePath}";

            return result;
        }

        public async Task<IEnumerable<MediaFileDto>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            var baseUrl = _storage.GetPublicBaseUrl();
            var result = _mapper.Map<List<MediaFileDto>>(items);

            foreach (var file in result)
                file.FileUrl = $"{baseUrl}/{file.FilePath}";

            return result;
        }

        public async Task<List<MediaFileDto>> GetInFolderAsync(Guid folderId)
        {
            var items = await _repo.GetByFolderIdAsync(folderId);
            var baseUrl = _storage.GetPublicBaseUrl();
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

            var baseUrl = _storage.GetPublicBaseUrl();
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
    }
}
