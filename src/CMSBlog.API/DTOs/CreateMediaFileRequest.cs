namespace CMSBlog.API.DTOs
{
    public class CreateMediaFileRequest
    {
        public IFormFile File { get; set; } = null!;
        public Guid? FolderId { get; set; }
    }

}
