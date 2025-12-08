using Microsoft.AspNetCore.Mvc;

namespace CMSBlog.API.DTOs
{
    public class UpdateMediaFileRequest
    {
        [FromForm]
        public IFormFile? File { get; set; }

        [FromForm] 
        public string FileName { get; set; } = null!;
        public string? Description { get; set; }
        public string? AltText { get; set; }
        public string? Caption { get; set; }
    }
}
