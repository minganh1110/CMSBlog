using Microsoft.AspNetCore.Mvc;

namespace CMSBlog.API.DTOs
{
    public class UpdateMediaFileRequest
    {
        public IFormFile? File { get; set; }

        public string FileName { get; set; } = null!;
        public string? Description { get; set; }
        public string? AltText { get; set; }
        public string? Caption { get; set; }
        public Guid? FolderId { get; set; }
    }
}
