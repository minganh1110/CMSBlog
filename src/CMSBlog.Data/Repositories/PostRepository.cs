using AutoMapper;
using CMSBlog.Core.Domain.Content;
using CMSBlog.Core.Models;
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
    public class PostRepository : RepositoryBase<Post, Guid>, IPostRepository
    {
        private readonly IMapper _mapper;
        public PostRepository(CMSBlogContext context,IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public Task<List<Post>> GetPostsByAuthorAsync(int count)
        {
            return _context.Posts.OrderByDescending(x => x.ViewCount).Take(count).ToListAsync();


        }

        public async Task<List<PostInListDto>> GetPostsPagingAsync(string? keyword, Guid? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var query = _context.Posts.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }
            var totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated)
                         .Skip((pageIndex - 1) * pageSize)
                         .Take(pageSize);

            return new PagedResult<PostInListDto>
            {
         
                Results = await _mapper.ProjectTo<PostInListDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            }.Results;
        }
    }
}
