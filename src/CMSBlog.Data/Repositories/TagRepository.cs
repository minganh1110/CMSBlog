using AutoMapper;
using CMSBlog.Core.Domain.Content;
using CMSBlog.Core.Models.Content;
using CMSBlog.Core.Repositories;
using CMSBlog.Data.SeedWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Data.Repositories
{
    public class TagRepository : RepositoryBase<Tag, Guid>, ITagRepository
    {
        private readonly IMapper _mapper;
        public TagRepository(CMSBlogContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<TagDto?> GetBySlug(string slug)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Slug == slug);
            if (tag == null) return null;
            return _mapper.Map<TagDto?>(tag);
        }
    }
}
