using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Data
{
    public class CMSBlogContextFactory : IDesignTimeDbContextFactory<CMSBlogContext>
    {
        public CMSBlogContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
            var builder = new DbContextOptionsBuilder<CMSBlogContext>();
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new CMSBlogContext(builder.Options);
        }
    }
}
