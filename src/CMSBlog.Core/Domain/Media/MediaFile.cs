using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMSBlog.Core.Domain.Media
{
    [Table("MediaFiles")]
    [Index(nameof(ID), IsUnique = true)]
    public class MediaFile
    {
        public Guid ID { get; set; } 
        public string FileName { get; set; } =null!;
        public string? Description { get; set; }
        public string SlugName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string FileExtension { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public MediaType MediaType { get; set; }
        public long FileSize { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public double? Duration { get; set; }
        //public Guid? FolderId { get; set; }
        public Guid? UploadedByUserId { get; set; }
        public Guid? ModifiedByUserId { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublic { get; set; }
        public string? AltText { get; set; }
        public string? Caption { get; set; }
        public string Provider { get; set; } = null!; // e.g., "Local", "AWS", "Azure", etc.

        //navigation properties
        //public MediaFolder? Folder { get; set; }
        public ICollection<MediaFileTag>? MediaFileTags { get; set; }
        public ICollection<MediaFileFolderLink>? FileFolderLinks { get; set; }
        
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
