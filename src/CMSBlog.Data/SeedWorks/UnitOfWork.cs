using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMSBlog.Core.Repositories;
using CMSBlog.Core.SeedWorks;
using CMSBlog.Data.Repositories;

namespace CMSBlog.Data.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CMSBlogContext _context;
        public UnitOfWork(CMSBlogContext context,IMapper mapper)
        {
            _context = context;
            Posts = new PostRepository(context,mapper);
            PostCategories = new PostCategoryRepository(context, mapper);
        }
        public IPostRepository Posts { get; private set; }
        public IPostCategoryRepository PostCategories { get; private set; }
        public async Task<int> CompleteAsync()
        {
           return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
