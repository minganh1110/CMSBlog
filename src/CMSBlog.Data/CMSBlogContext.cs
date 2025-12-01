using CMSBlog.Core.Application.DTOs.Media;
using CMSBlog.Core.Domain.Content;
using CMSBlog.Core.Domain.Identity;
using CMSBlog.Core.Domain.Media;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Data
{
    public class CMSBlogContext : IdentityDbContext<AppUser,AppRole,Guid>
    {
        public CMSBlogContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostActivityLog> PostActivityLogs { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<PostInSeries> PostInSeries { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<MediaTag> MediaTags { get; set; }
        public DbSet<MediaFileTag> MediaFileTags { get; set; }
        public DbSet<MediaFolder> MediaFolders { get; set; }
        public DbSet<MediaFileFolderLink> MediaFileFolderLinks { get; set; }
        public DbSet<MediaSetting> MediaSettings { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Tên bảng tùy chỉnh Identity
            base.OnModelCreating(builder);
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);

            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims")
            .HasKey(x => x.Id);

            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles")
            .HasKey(x => new { x.RoleId, x.UserId });

            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens")
               .HasKey(x => new { x.UserId });

            //Áp dụng tất cả IEntityTypeConfiguration<T>
            builder.ApplyConfigurationsFromAssembly(typeof(CMSBlogContext).Assembly);

            builder.Entity<MediaSetting>().HasData(new MediaSetting
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ActiveProvider = "Local",
                UpdatedAt = DateTime.UtcNow
            });


        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
               .Entries()
               .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                var dateCreatedProp = entityEntry.Entity.GetType().GetProperty("DateCreated");
                if (entityEntry.State == EntityState.Added
                    && dateCreatedProp != null)
                {
                    dateCreatedProp.SetValue(entityEntry.Entity, DateTime.Now);
                }
                var ModifiedDateProp = entityEntry.Entity.GetType().GetProperty("DateModified");
                if (entityEntry.State == EntityState.Modified
                    && ModifiedDateProp != null)
                {
                    ModifiedDateProp.SetValue(entityEntry.Entity, DateTime.Now);
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
