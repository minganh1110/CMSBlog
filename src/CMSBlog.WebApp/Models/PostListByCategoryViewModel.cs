using CMSBlog.Core.Models;
using CMSBlog.Core.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace CMSBlog.WebApp.Models
{
    public class PostListByCategoryViewModel
    {
        public PostCategoryDto Category { get; set; }
        public PagedResult<PostInListDto> Posts { get; set; }
    }
}
