using CMSBlog.Core.Models;
using CMSBlog.Core.Models.Content;

namespace CMSBlog.WebApp.Models
{
    public class PostListByTagViewModel
    {
        public TagDto Tag { get; set; }
        public PagedResult<PostInListDto> Posts { get; set; }

    }
}
