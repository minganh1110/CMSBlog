using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMSBlog.Core.SeedWorks;

namespace CMSBlog.Data.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CMSBlogContext _context;
        public UnitOfWork(CMSBlogContext context)
        {
            _context = context;
        }
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
