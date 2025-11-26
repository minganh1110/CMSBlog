using CMSBlog.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.Interfaces.Media
{
    public interface IMediaFileRepository
    {
        Task<MediaFile> AddAsync(MediaFile entity);
        Task<MediaFile?> GetByIdAsync(Guid id);
        Task<List<MediaFile>> GetByFolderIdAsync(Guid folderId);
        Task<IEnumerable<MediaFile>> GetAllAsync();
        Task SaveChangesAsync();
        Task DeleteAsync(Guid id);
        Task UpdateAsync(MediaFile entity);

    }
}
