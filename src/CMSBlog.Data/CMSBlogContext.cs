using CMSBlog.Core.Domain.Content;
using CMSBlog.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public DbSet<MediaFiles> MediaFiles { get; set; }
        public DbSet<MediaTags> MediaTags { get; set; }
        public DbSet<MediaFileTags> MediaFileTags { get; set; }
        public DbSet<MediaFolders> MediaFolders { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);

            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims")
            .HasKey(x => x.Id);

            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles")
            .HasKey(x => new { x.RoleId, x.UserId });

            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens")
               .HasKey(x => new { x.UserId });

            builder.Entity<MediaFiles>(entity =>
            {
                entity.ToTable("MediaFiles");

                entity.HasKey(x => x.ID);

                entity.Property(x => x.FileName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(x => x.SlugName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(x => x.FilePath)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(x => x.FileExtension)
                    .HasMaxLength(50);

                entity.Property(x => x.MediaType)
                    .HasConversion<int?>();

                entity.Property(x => x.UploadedAt)
                    .IsRequired();

                entity.Property(x => x.IsDeleted)
                    .HasDefaultValue(false);

                // Folder quan hệ 1-n (nếu có MediaFolders entity)
                entity.HasOne<MediaFolders>()
                      .WithMany()
                      .HasForeignKey(x => x.FolderId)
                      .OnDelete(DeleteBehavior.SetNull);

                // Quan hệ với AppUser (User upload file)
                entity.HasOne<AppUser>()
                      .WithMany()
                      .HasForeignKey(x => x.UploadedByUserId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<MediaFolders>(entity =>
            {
                entity.ToTable("MediaFolders");
                entity.HasKey(x => x.ID);
                entity.Property(x => x.FolderName)
                    .IsRequired()
                    .HasMaxLength(250);
                entity.Property(x => x.SlugName)
                    .IsRequired()
                    .HasMaxLength(250);
                entity.Property(x => x.CreatedAt)
                    .IsRequired();
                // Quan hệ cha-con với chính nó
                builder.Entity<MediaFolders>()
                    .HasOne<MediaFolders>()
                    .WithMany()
                    .HasForeignKey(x => x.ParentFolderId)
                    .OnDelete(DeleteBehavior.NoAction);


            });

            builder.Entity<MediaTags>(entity =>
            {
                entity.ToTable("MediaTags");
                entity.HasKey(x => x.ID);
                entity.Property(x => x.SlugName)
                    .IsRequired()
                    .HasMaxLength(250);
                entity.Property(x => x.TagName)
                    .IsRequired()
                    .HasMaxLength(250);
                entity.Property(x => x.Description)
                    .HasMaxLength(1000);
                entity.Property(x => x.CreatedAt)
                    .IsRequired();
            });

            builder.Entity<MediaFileTags>(entity =>
            {
                entity.ToTable("MediaFileTags");
                entity.HasKey(x => new { x.MediaFileId, x.MediaTagId });
                entity.HasOne<MediaFiles>()
                      .WithMany()
                      .HasForeignKey(x => x.MediaFileId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne<MediaTags>()
                      .WithMany()
                      .HasForeignKey(x => x.MediaTagId)
                      .OnDelete(DeleteBehavior.Cascade);
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
                var ModifiedDateProp = entityEntry.Entity.GetType().GetProperty("ModifiedDate");
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
