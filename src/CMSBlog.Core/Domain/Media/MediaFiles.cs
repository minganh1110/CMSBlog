using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMSBlog.Core.Domain.Media
{
    [Table("MediaFiles")]
    [Index(nameof(ID), IsUnique = true)]
    public class MediaFiles
    {
        public Guid ID { get; set; } 
        public string FileName { get; set; } =null!;
        public string? Description { get; set; }
        public string SlugName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string? FileExtension { get; set; } = null!;
        public int? MediaType { get; set; }
        public long FileSize { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public double? Duration { get; set; }
        public Guid? FolderId { get; set; }
        public Guid? UploadedByUserId { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool IsDeleted { get; set; }

        public MediaFolders? Folder { get; set; }
        public ICollection<MediaFileTags>? MediaFileTags { get; set; }
        
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
