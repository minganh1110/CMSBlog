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
    public class MediaFoldersConfiguration : IEntityTypeConfiguration<MediaFolders>
    {
        public void Configure(EntityTypeBuilder<MediaFolders> entity)
        {
            entity.ToTable("MediaFolders");

            entity.HasKey(x => x.ID);

            entity.Property(x => x.FolderName).IsRequired().HasMaxLength(250);
            entity.Property(x => x.SlugName).IsRequired().HasMaxLength(250);

            entity.HasOne(x => x.ParentFolder)
                  .WithMany(x => x.ChildFolders)
                  .HasForeignKey(x => x.ParentFolderId)
                  .OnDelete(DeleteBehavior.NoAction);
        }
    }

}
