using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSBlog.Core.Domain.Content
{
    public class MediaFileTags
    {
        [ForeignKey("MediaFiles")]
        public Guid MediaFileId { get; set; }
        [ForeignKey("MediaTags")]
        public Guid MediaTagId { get; set; }
    }
}
