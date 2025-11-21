using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using CMSBlog.Core.Domain.Media;

namespace CMSBlog.Core.Models.Media
{
    internal class Class1
    {
        public string FileName { get; set; } = null!;
        public string? Description { get; set; } 
        public string SlugName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string FileExtension { get; set; } = null!;
        public int? MediaType { get; set; }
        public long FileSize { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public double? Duration { get; set; }
        protected Guid? FolderId { get; set; }
        protected Guid? UploadedByUserId { get; set; }
    }
}
