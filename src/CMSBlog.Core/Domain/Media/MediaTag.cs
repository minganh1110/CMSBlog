using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSBlog.Core.Domain.Media
{
    public class MediaTag
    {
        public Guid ID { get; set; }
        public string SlugName { get; set; } = null!;
        public string TagName { get; set; } = null!;
        public string? Description { get; set; } 
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set;} = DateTime.Now;

        public ICollection<MediaFileTag>? MediaFileTags { get; set; }

    }
}
