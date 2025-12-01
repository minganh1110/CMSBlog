using CMSBlog.Core.Application.DTOs.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.Interfaces.Media
{
    public interface IMediaTagService
    {
        Task<MediaTagDto> CreateAsync(CreateMediaTagDto dto);
        Task<bool> UpdateAsync(Guid id, UpdateMediaTagDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<List<MediaTagDto>> GetAllAsync();
    }

}
