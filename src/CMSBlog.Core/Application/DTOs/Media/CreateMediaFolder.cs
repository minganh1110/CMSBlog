using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.DTOs.Media
{
    public class CreateMediaFolderDto
    {
        public string FolderName { get; set; } = null!;
        public Guid? ParentId { get; set; }
    }
}
