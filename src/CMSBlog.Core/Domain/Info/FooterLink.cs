using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSBlog.Core.Domain.Info
{
    [Table("FooterLinks")]
    public class FooterLink
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(250)]
        public required string Label { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Url { get; set; }

        [MaxLength(100)]
        public string? Icon { get; set; }

        public bool TargetBlank { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}

