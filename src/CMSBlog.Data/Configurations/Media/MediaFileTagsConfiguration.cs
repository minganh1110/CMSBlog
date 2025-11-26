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
    public class MediaFileTagsConfiguration : IEntityTypeConfiguration<MediaFileTag>
    {
        public void Configure(EntityTypeBuilder<MediaFileTag> entity)
        {
            entity.ToTable("MediaFileTags");

            entity.HasKey(x => new { x.MediaFileId, x.MediaTagId });

            entity.HasOne(x => x.MediaFile)
                  .WithMany(x => x.MediaFileTags)
                  .HasForeignKey(x => x.MediaFileId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.MediaTag)
                  .WithMany(x => x.MediaFileTags)
                  .HasForeignKey(x => x.MediaTagId)
                  .OnDelete(DeleteBehavior.Cascade);


            entity.HasIndex(x => new { x.MediaFileId, x.MediaTagId }).IsUnique(true);
        }
    }

}
