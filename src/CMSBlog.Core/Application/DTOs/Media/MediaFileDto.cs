using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMSBlog.Core.Domain.Media;
using System.ComponentModel.DataAnnotations;


namespace CMSBlog.Core.Application.DTOs.Media
{
    public class MediaFileDto
    {
        [Key]
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "FileName is required.")]
        public string FileName { get; set; } = null!;

        [Required(ErrorMessage = "FilePath is required.")]
        public string FilePath { get; set; } = null!;
        public string? Description { get; set; }
        public long FileSize { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid? FolderId { get; set; }
        public string? FolderName { get; set; }
        public string FileUrl { get; set; } = null!;
        public MediaType MediaType { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public MediaFormats? Formats { get; set; }
        public string Url => Formats?.Small?.Url ?? Formats?.Thumbnail?.Url ?? "";
        public string? AltText { get; set; }
        public string? Caption { get; set; }
    }

}
