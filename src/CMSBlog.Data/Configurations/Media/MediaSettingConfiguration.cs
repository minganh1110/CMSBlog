using CMSBlog.Core.Application.DTOs.Media;
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
    internal class MediaSettingConfiguration : IEntityTypeConfiguration<MediaSetting>
    {
        public void Configure(EntityTypeBuilder<MediaSetting> entity)
        {
            entity.ToTable("MediaSettings");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("Id");
            entity.Property(x => x.ActiveProvider).IsRequired().HasMaxLength(100);
        }
    }
}
