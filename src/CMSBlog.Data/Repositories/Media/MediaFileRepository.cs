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

        public async Task<MediaFiles> AddAsync(MediaFiles entity)
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

        public async Task<IEnumerable<MediaFiles>> GetAllAsync()
        {
            return await _db.MediaFiles.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<MediaFiles?> GetByIdAsync(Guid id)
        {
            return await _db.MediaFiles.FindAsync(id);
        }
    }
}
