using CMSBlog.Core.Domain.Menu;
using CMSBlog.Core.SeedWorks;

namespace CMSBlog.Core.Repositories
{
    public interface IMenuRepository : IRepository<MenuItem, Guid>
    {
    }
}
