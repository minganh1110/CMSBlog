using CMSBlog.Core.Application.DTOs.Media;
using CMSBlog.Core.Application.Interfaces.Media;
using CMSBlog.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.Services.Media
{
    public class MediaFileService : IMediaFileService
    {
        private readonly IMediaFileRepository _repo;
        private readonly IStorageServicee _storage;

        public MediaFileService(IMediaFileRepository repo, IStorageServicee storage)
        {
            _repo = repo;
            _storage = storage;
        }

        public async Task<MediaFileDto> UploadAsync(CreatedMediaFileDto dto, CancellationToken ct = default)
        {
            if (dto.FileContent.Length == 0)
                throw new ArgumentException("File is empty");

            var slug = GenerateSlug(Path.GetFileNameWithoutExtension(dto.FileName));
            var ext = Path.GetExtension(dto.FileName);
            var fileName = $"{slug}{ext}";

            // Lưu file thông qua interface
            var storagePath = await _storage.SaveFileAsync(dto.FileContent, fileName, ct);

            var entity = new MediaFiles
            {
                ID = Guid.NewGuid(),
                FileName = dto.FileName,
                SlugName = slug,
                FilePath = storagePath,
                FileExtension = ext,
                FileSize = dto.FileContent.Length,
                FolderId = dto.FolderId,
                DateCreated = DateTime.UtcNow
            };

            await _repo.AddAsync(entity);

            var baseUrl = _storage.GetPublicBaseUrl();
            return new MediaFileDto
            {
                Id = entity.ID,
                FileName = entity.FileName,
                FilePath = storagePath,
                Url = $"{baseUrl}/{storagePath.TrimStart('/')}",
                FileSize = entity.FileSize,
                FileExtension = entity.FileExtension,
                DateCreated = entity.DateCreated.Value,
                FolderId = entity.FolderId
            };
        }

        public async Task<IEnumerable<MediaFileDto>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            var baseUrl = _storage.GetPublicBaseUrl();
            return items.Select(x => new MediaFileDto
            {
                Id = x.ID,
                FileName = x.FileName,
                FilePath = x.FilePath,
                Url = $"{baseUrl}/{x.FilePath.TrimStart('/')}",
                FileSize = x.FileSize,
                FileExtension = x.FileExtension,
                DateCreated = x.DateCreated ?? DateTime.UtcNow,
                FolderId = x.FolderId
            });
        }

        public async Task<MediaFileDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            var baseUrl = _storage.GetPublicBaseUrl();
            return new MediaFileDto
            {
                Id = entity.ID,
                FileName = entity.FileName,
                FilePath = entity.FilePath,
                Url = $"{baseUrl}/{entity.FilePath.TrimStart('/')}",
                FileSize = entity.FileSize,
                FileExtension = entity.FileExtension,
                DateCreated = entity.DateCreated ?? DateTime.UtcNow,
                FolderId = entity.FolderId
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
        }

        private string GenerateSlug(string input)
        {
            return input.ToLowerInvariant().Replace(" ", "-");
        }
    }
}
