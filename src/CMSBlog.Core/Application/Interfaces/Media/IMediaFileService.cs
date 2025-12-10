using CMSBlog.Core.Application.DTOs.Media;
using CMSBlog.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.Interfaces.Media
{
    public interface IMediaFileService
    {
        Task<MediaFileDto> UploadAsync(CreatedMediaFileDto dto, CancellationToken ct = default);
        Task<List<MediaFileDto>> GetAllAsync();
        Task<MediaFileDto?> GetByIdAsync(Guid id);
        Task<List<MediaFileDto>> GetInFolderAsync(Guid folderId);
        Task<bool> UpdateAsync(Guid id, UpdateMediaFileDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<string> UploadAsync(byte[] content, string fileName);
        Task<MediaFormat> GenerateFormatAsync(Stream original, string NameFile,
            int targetWidth, string prefix, string ext, CancellationToken ct = default);
        Task<Dictionary<string, MediaFormat>> GenerateAllFormats(Stream original, string NameFile, string ext);
        Task<bool> ReplaceMediaAsync(Guid id, byte[] newContent, CancellationToken ct = default);
        Task<bool> MoveToFolderAsync(Guid fileId, Guid newFolderId);
        public MediaType DetectMediaType(string mime);
    }
}
