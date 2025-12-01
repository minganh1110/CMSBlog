using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.DTOs.Media
{
    public class MediaSetting
    {
        public Guid Id { get; set; }
        public string ActiveProvider { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
    }
}
