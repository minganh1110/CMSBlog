using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSBlog.Core.Domain.Media
{
    public class MediaFileTag
    {
        public Guid MediaFileId { get; set; }
        public Guid MediaTagId { get; set; }

        public MediaFile MediaFile { get; set; } = null!;
        public MediaTag MediaTag { get; set; } = null!;
    }
}
