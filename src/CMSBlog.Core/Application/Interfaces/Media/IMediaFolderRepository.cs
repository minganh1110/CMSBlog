using CMSBlog.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.Interfaces.Media
{
    public interface IMediaFolderRepository
    {
        Task AddAsync(MediaFolder entity);
        Task<MediaFolder?> GetByIdAsync(Guid id);
        Task<MediaFolder?> GetByIdWithChildrenAsync(Guid id); // include children if needed
        Task<List<MediaFolder>> GetAllAsync();
        Task<List<MediaFolder>> GetDescendantsAsync(string pathPrefix); // pathPrefix = "/1/2"
        Task UpdateAsync(MediaFolder entity);
        Task DeleteRangeAsync(IEnumerable<MediaFolder> entities);
        Task SaveChangesAsync();

        void Attach<TEntity>(TEntity entity) where TEntity : class;
        void SetPropertyModified<TEntity, TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression)
            where TEntity : class;

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }

}
