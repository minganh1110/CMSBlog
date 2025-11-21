using CMSBlog.Core.Application.DTOs.Media;
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
        Task DeleteAsync(Guid id);
    }
}
