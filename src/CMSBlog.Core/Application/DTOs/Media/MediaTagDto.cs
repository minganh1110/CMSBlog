using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.DTOs.Media
{
    public class MediaTagDto
    {
        public Guid ID { get; set; }
        public string TagName { get; set; } = null!;
        public string SlugName { get; set; } = null!;
    }
}
