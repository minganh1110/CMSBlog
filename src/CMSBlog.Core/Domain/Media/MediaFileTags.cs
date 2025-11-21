using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSBlog.Core.Domain.Media
{
    public class MediaFileTags
    {
        public Guid MediaFileId { get; set; }
        public Guid MediaTagId { get; set; }

        public MediaFiles MediaFile { get; set; } = null!;
        public MediaTags MediaTag { get; set; } = null!;
    }
}
