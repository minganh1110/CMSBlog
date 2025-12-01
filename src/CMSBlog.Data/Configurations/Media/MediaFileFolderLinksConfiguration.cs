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
    public class MediaFileFolderLinksConfiguration : IEntityTypeConfiguration<MediaFileFolderLink>
    {
        public void Configure (EntityTypeBuilder<MediaFileFolderLink> entity)
        {
            entity.ToTable(nameof (MediaFileFolderLink));

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.HasKey(x =>new { x.MediaFolderId, x.MediaFileId });
            entity.HasOne(x => x.MediaFile)
                  .WithMany(x=> x.FileFolderLinks)
                  .HasForeignKey(x => x.MediaFileId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.MediaFolder)
                  .WithMany(x => x.FileFolderLinks)
                  .HasForeignKey(x => x.MediaFolderId)
                  .OnDelete(DeleteBehavior.Cascade);


            entity.HasIndex(x => new { x.MediaFileId , x.MediaFolderId }).IsUnique(true);
        }
    }
}
