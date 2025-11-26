using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.DTOs.Media
{
    public class CreateMediaTagDto
    {
        public string TagName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
