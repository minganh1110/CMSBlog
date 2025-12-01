using CMSBlog.Core.Application.Interfaces.Media;
using CMSBlog.Core.Domain.Media;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSBlog.Data.Repositories.Media
{
    public class MediaFileRepository : IMediaFileRepository
    {
        private readonly CMSBlogContext _db;
        public MediaFileRepository(CMSBlogContext db)
        {
            _db = db;
        }

        public async Task<MediaFile> AddAsync(MediaFile entity)
        {
            _db.MediaFiles.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.MediaFiles.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                await _db.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<MediaFile>> GetAllAsync(string providerName)
        {
            return await _db.MediaFiles
                    .Where(x => !x.IsDeleted && x.Provider == providerName)
                    .ToListAsync();
        }

        public async Task<List<MediaFile>> GetByFolderIdAsync(Guid folderId, string providerName)
        {
            return await _db.MediaFiles
                .Where(x =>
                    x.FileFolderLinks.Any(link => link.MediaFolderId == folderId)
                    && x.Provider == providerName
                    && !x.IsDeleted)
                .ToListAsync();
        }


        public async Task<MediaFile?> GetByIdAsync(Guid id)
        {
            return await _db.MediaFiles.FindAsync(id);
        }

        

        public async Task UpdateAsync(MediaFile entity)
        {
            _db.MediaFiles.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
