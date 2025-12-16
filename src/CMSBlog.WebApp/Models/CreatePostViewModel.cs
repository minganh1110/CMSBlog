using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CMSBlog.WebApp.Models
{
    public class CreatePostViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? ThumbnailImage { get; set; }
        public Guid CategoryId { get; set; }
        public SelectList? Categories { get; set; }
        public int SortOrder { get; set; }
        public string? SeoDescription { get; set; }
    }
}
