using CMSBlog.Core.Domain.Menu;
using CMSBlog.Core.Repositories;
using CMSBlog.Data.SeedWorks;
using Microsoft.EntityFrameworkCore;

namespace CMSBlog.Data.Repositories
{
    public class MenuRepository : RepositoryBase<MenuItem, Guid>, IMenuRepository
    {
        public MenuRepository(CMSBlogContext context) : base(context)
        {
        }
    }
}
