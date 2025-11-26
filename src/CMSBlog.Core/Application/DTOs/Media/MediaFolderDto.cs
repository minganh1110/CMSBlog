using CMSBlog.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.DTOs.Media
{
    public class MediaFolderDto
    {
        public Guid Id { get; set; }
        public string FolderName { get; set; } = null!;
        public Guid? ParentId { get; set; }

        public List<MediaFolderDto>? Children { get; set; }
        public List<MediaFileDto>? Files { get; set; }
    }

}
