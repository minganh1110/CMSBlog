using Microsoft.AspNetCore.Mvc;

namespace CMSBlog.API.DTOs
{
    public class CreateMediaFileRequest
    {
        [FromForm]
        public IFormFile File { get; set; } = null!;

        [FromForm]
        public Guid? FolderId { get; set; }
    }

}
