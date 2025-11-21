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
    public class MediaFileTagsConfiguration : IEntityTypeConfiguration<MediaFileTags>
    {
        public void Configure(EntityTypeBuilder<MediaFileTags> entity)
        {
            entity.ToTable("MediaFileTags");

            entity.HasKey(x => new { x.MediaFileId, x.MediaTagId });

            entity.HasOne(x => x.MediaFile)
                  .WithMany(x => x.MediaFileTags)
                  .HasForeignKey(x => x.MediaFileId);

            entity.HasOne(x => x.MediaTag)
                  .WithMany(x => x.MediaFileTags)
                  .HasForeignKey(x => x.MediaTagId);
        }
    }

}
