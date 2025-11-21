using CMSBlog.Core.Domain.Identity;
using CMSBlog.Core.Domain.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Data.Configurations.Media
{
    public class MediaFilesConfiguration : IEntityTypeConfiguration<MediaFiles>
    {
        public void Configure(EntityTypeBuilder<MediaFiles> entity)
        {
            entity.ToTable("MediaFiles");

            entity.HasKey(x => x.ID);

            entity.Property(x => x.FileName).IsRequired().HasMaxLength(250);
            entity.Property(x => x.SlugName).IsRequired().HasMaxLength(250);
            entity.Property(x => x.FilePath).IsRequired().HasMaxLength(250);
            entity.Property(x => x.FileExtension).HasMaxLength(50);

            entity.Property(x => x.IsDeleted).HasDefaultValue(false);
            entity.Property(x => x.DateCreated).IsRequired();

            // Quan hệ với MediaFolders
            entity.HasOne(x => x.Folder)
                  .WithMany(f => f.MediaFiles)
                  .HasForeignKey(x => x.FolderId)
                  .OnDelete(DeleteBehavior.SetNull);

            // Quan hệ User Upload
            entity.HasOne<AppUser>()
                  .WithMany()
                  .HasForeignKey(x => x.UploadedByUserId)
                  .OnDelete(DeleteBehavior.SetNull);
        }
    }

}
