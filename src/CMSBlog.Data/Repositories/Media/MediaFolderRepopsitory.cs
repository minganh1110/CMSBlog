using CMSBlog.Core.Application.Interfaces;
using CMSBlog.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CMSBlog.Data.Repositories.Media
{
    public class MediaFolderRepository : IRepository<MediaFolder>
    {
        private readonly CMSBlogContext _context;

        public MediaFolderRepository(CMSBlogContext context)
        {
            _context = context;
        }

        public async Task<MediaFolder> AddAsync(MediaFolder entity)
        {
            _context.MediaFolders.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<MediaFolder?> GetByIdAsync(Guid id)
        {
            return await _context.MediaFolders
                .Include(f => f.ChildFolders)
                .FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<List<MediaFolder>> GetAllAsync()
        {
            return await _context.MediaFolders
                .Include(f => f.ChildFolders)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(MediaFolder entity)
        {
            _context.MediaFolders.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task SaveChangesAsync()
            => _context.SaveChangesAsync();
    }

}
