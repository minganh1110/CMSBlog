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
    public class MediaTagsConfiguration : IEntityTypeConfiguration<MediaTag>
    {
        public void Configure(EntityTypeBuilder<MediaTag> entity)
        {
            entity.ToTable("MediaTags");
            entity.HasKey(x => x.ID);
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.Property(x => x.TagName).IsRequired().HasMaxLength(250);
            entity.Property(x => x.SlugName).IsRequired().HasMaxLength(250);

            entity.Property(x => x.DateCreated)
                   .HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(x => x.SlugName).IsUnique(true);
        }
    }

}
