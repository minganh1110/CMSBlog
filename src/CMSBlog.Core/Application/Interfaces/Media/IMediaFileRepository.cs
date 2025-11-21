using CMSBlog.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.Interfaces.Media
{
    public interface IMediaFileRepository
    {
        Task<MediaFiles> AddAsync(MediaFiles entity);
        Task<MediaFiles?> GetByIdAsync(Guid id);
        Task<IEnumerable<MediaFiles>> GetAllAsync();
        Task DeleteAsync(Guid id);
    }
}
