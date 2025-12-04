using CMSBlog.Core.Domain.Content;
using CMSBlog.Core.Models.Content;
using CMSBlog.Core.SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Repositories
{
    public interface ITagRepository : IRepository<Tag, Guid>
    {
        Task<TagDto> GetBySlug(string slug);
    }
}
