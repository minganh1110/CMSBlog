using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMSBlog.Core.Domain.Content
{
    [Table("MediaFiles")]
    [Index(nameof(ID), IsUnique = true)]
    public class MediaFiles
    {
        [Key]
        public Guid ID { get; set; } // Guid có thể null

        [Required(ErrorMessage = "Tên file là bắt buộc")]
        [MaxLength(250)]
        public string FileName { get; set; } =null!;

        [Required(ErrorMessage = "Slug file là bắt buộc")]
        [MaxLength(250)]
        public string SlugName { get; set; } = null!;

        [Required(ErrorMessage = "Đường dẫn file là bắt buộc")]
        [MaxLength(500)]
        public string FilePath { get; set; } = null!;
        public string FileExtension { get; set; } = null!;


        [MaxLength(100)]
        public int? MediaType { get; set; }

        public long FileSize { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public double? Duration { get; set; }

        public Guid? FolderId { get; set; }
        public Guid? UploadedByUserId { get; set; }

        public DateTime UploadedAt { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool IsDeleted { get; set; }


    }

    public enum MediaType
    {
        Image = 1,
        Video = 2,
        Audio = 3,
        Document = 4,
        Other = 5
    }
}
