using CMSBlog.Core.Application.DTOs.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.Interfaces.Media
{
    public interface IMediaFolderService
    {
        Task<MediaFolderDto> CreateAsync(CreateMediaFolderDto dto);
        Task<MediaFolderDto?> GetByIdAsync(Guid id);
        Task<List<MediaFolderDto>> GetAllAsync();
        Task<bool> UpdateAsync(Guid id, UpdateMediaFolderDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
