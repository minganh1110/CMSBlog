using System;

namespace CMSBlog.Core.Application.DTOs.Media
{
    public class CreatedMediaFileDto
    {
        public byte[] FileContent { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public Guid? FolderId { get; set; }
    }
}
