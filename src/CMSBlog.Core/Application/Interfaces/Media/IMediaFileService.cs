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
        Task<IEnumerable<MediaFileDto>> GetAllAsync();
        Task<MediaFileDto?> GetByIdAsync(Guid id);
        Task<List<MediaFileDto>> GetInFolderAsync(Guid folderId);
        Task<bool> UpdateAsync(Guid id, UpdateMediaFileDto dto);
        Task<bool> DeleteAsync(Guid id);
        public MediaType DetectMediaType(string mime);
    }
}
