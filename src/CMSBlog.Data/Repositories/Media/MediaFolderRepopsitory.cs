using CMSBlog.Core.Application.Interfaces;
using CMSBlog.Core.Domain.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CMSBlog.Data.Repositories.Media
{
    using CMSBlog.Core.Application.Interfaces.Media;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;

    public class MediaFolderRepository : IMediaFolderRepository
    {
        private readonly CMSBlogContext _db;
        private IDbContextTransaction? _transaction;
        public MediaFolderRepository(CMSBlogContext db) => _db = db;

        public async Task AddAsync(MediaFolder entity)
        {
            await _db.MediaFolders.AddAsync(entity);
        }

        public async Task<MediaFolder?> GetByIdAsync(Guid id)
        {
            return await _db.MediaFolders.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<MediaFolder?> GetByIdWithChildrenAsync(Guid id, string providerName)
        {
            return await _db.MediaFolders
                .Where(f => f.Id == id)
                .Include(f => f.ChildFolders)
                .Include(f => f.FileFolderLinks
                    .Where( link => link.MediaFile.Provider == providerName))
                    .ThenInclude(link => link.MediaFile)

                .FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<List<MediaFolder>> GetAllAsync()
        {
            return await _db.MediaFolders.OrderBy(f => f.Path).ToListAsync();
        }

        public async Task<List<MediaFolder>> GetDescendantsAsync(string pathPrefix)
        {
            // match pathPrefix + "/%" OR exactly equal? For descendants only:
            var like = pathPrefix.TrimEnd('/') + "/%";
            return await _db.MediaFolders
                .Where(f => EF.Functions.Like(f.Path, like))
                .ToListAsync();
        }

        public async Task  UpdateAsync(MediaFolder entity)
        {
            
            _db.MediaFolders.Update(entity);
            _db.Entry(entity).Property(x => x.PathId).IsModified = false;

            await _db.SaveChangesAsync();
            //return await Task.CompletedTask;
        }

        public Task DeleteRangeAsync(IEnumerable<MediaFolder> entities)
        {
            _db.MediaFolders.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
                return;

            _transaction = await _db.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction == null)
                return;

            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null)
                return;

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            _db.Attach(entity);
        }

        public void SetPropertyModified<TEntity, TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression)
            where TEntity : class
        {
            _db.Entry(entity).Property(propertyExpression).IsModified = true;
        }


        public Task SaveChangesAsync() => _db.SaveChangesAsync();
    }


}
