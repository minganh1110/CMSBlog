using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSBlog.Core.Domain.Content
{
    public class MediaFolders
    {
        [Key]
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Tên thư mục là bắt buộc")]
        [MaxLength(250)]
        public string FolderName { get; set; } = null!;

        [Required(ErrorMessage = "Slug thư mục là bắt buộc")]
        [MaxLength(250)]
        public string SlugName { get; set; } = null!;

        [ForeignKey("Folder")]
        public Guid? ParentFolderId { get; set; }

        public Guid CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }


    }
}
