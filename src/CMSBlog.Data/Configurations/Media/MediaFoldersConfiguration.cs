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
    public class MediaFoldersConfiguration : IEntityTypeConfiguration<MediaFolder>
    {
        public void Configure(EntityTypeBuilder<MediaFolder> entity)
        {
            entity.ToTable("MediaFolders");

            entity.HasKey(x => x.ID);

            entity.Property(x => x.FolderName).IsRequired().HasMaxLength(250);
            entity.Property(x => x.SlugName).IsRequired().HasMaxLength(250);
            entity.Property(x => x.FullPath).IsRequired().HasMaxLength(500);

            entity.Property(x => x.DateCreated)
                .HasDefaultValueSql("GETUTCDATE()");
            entity.Property(x => x.DateModified)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(x => x.ParentFolder)
                  .WithMany(x => x.ChildFolders)
                  .HasForeignKey(x => x.ParentFolderId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
