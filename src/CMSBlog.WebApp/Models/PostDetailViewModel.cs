using CMSBlog.Core.Models.Content;

namespace CMSBlog.WebApp.Models
{
    public class PostDetailViewModel
    {
        public PostDto Post { get; set; }
        public PostCategoryDto Category { get; set; }
        public List<TagDto> Tags { get; set; }
    }
}
