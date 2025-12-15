using CMSBlog.Core.Domain.Content;
using CMSBlog.Core.Models;
using CMSBlog.Core.Models.Content;
using CMSBlog.Core.SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Repositories
{
    public interface IPostCategoryRepository : IRepository<PostCategory, Guid>
    {
        Task<PagedResult<PostCategoryDto>> GetAllPaging(string? keyword, int pageIndex = 1, int pageSize = 10);
        Task<bool> HasPost(Guid categoryId);
        Task<PostCategoryDto> GetBySlug(string slug);

    }
}
