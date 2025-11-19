using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSBlog.Core.Domain.Content
{
    public class MediaTags
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string SlugName { get; set; } = null!;

        [Required]
        public string TagName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
