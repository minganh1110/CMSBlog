using System;

using System.ComponentModel.DataAnnotations;

namespace CMSBlog.Core.Application.DTOs.Media
{
    public class CreatedMediaFileDto
    {
        public byte[] FileContent { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public Guid? FolderId { get; set; }

        [Required(ErrorMessage ="MimeType là bắt buộc")]
        public string MimeType { get; set; } = null!;
    }
}
