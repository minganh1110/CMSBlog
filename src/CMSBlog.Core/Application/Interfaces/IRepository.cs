using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();
        Task<bool> DeleteAsync(T entity);
        Task SaveChangesAsync();
    }

}
