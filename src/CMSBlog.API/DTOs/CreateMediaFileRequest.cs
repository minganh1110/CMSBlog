using Microsoft.AspNetCore.Mvc;

namespace CMSBlog.API.DTOs
{
    public class CreateMediaFilesRequest
    {
        [FromForm]
        public List<IFormFile> Files { get; set; } = new();

        [FromForm]
        public Guid? FolderId { get; set; }
    }


}
