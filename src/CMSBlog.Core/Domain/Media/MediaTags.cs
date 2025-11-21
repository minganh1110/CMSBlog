using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSBlog.Core.Domain.Media
{
    public class MediaTags
    {
        public Guid ID { get; set; }
        public string SlugName { get; set; } = null!;
        public string TagName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set;} = DateTime.Now;

        public ICollection<MediaFileTags> MediaFileTags { get; set; }

    }
}
