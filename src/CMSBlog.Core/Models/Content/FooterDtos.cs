using AutoMapper;
using CMSBlog.Core.Domain.Info;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMSBlog.Core.Models.Content
{
    public class FooterSettingsDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? CopyrightText { get; set; }
        public string? FooterNote { get; set; }
        public string? ContactEmail { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsActive { get; set; }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<FooterSettings, FooterSettingsDto>();
                CreateMap<CreateUpdateFooterSettingsRequest, FooterSettings>();
                CreateMap<FooterLink, FooterLinkDto>();
                CreateMap<CreateUpdateFooterLinkRequest, FooterLink>();
            }
        }
    }

    public class CreateUpdateFooterSettingsRequest
    {
        [MaxLength(250)]
        public string? Title { get; set; }

        [MaxLength(500)]
        public string? Subtitle { get; set; }

        [MaxLength(500)]
        public string? CopyrightText { get; set; }

        [MaxLength(500)]
        public string? FooterNote { get; set; }

        [MaxLength(250)]
        public string? ContactEmail { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(500)]
        public string? LogoUrl { get; set; }

        public bool IsActive { get; set; }
    }

    public class FooterLinkDto
    {
        public Guid Id { get; set; }
        public string Label { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string? Icon { get; set; }
        public bool TargetBlank { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateUpdateFooterLinkRequest
    {
        [Required]
        [MaxLength(250)]
        public string Label { get; set; } = default!;

        [Required]
        [MaxLength(500)]
        public string Url { get; set; } = default!;

        [MaxLength(100)]
        public string? Icon { get; set; }

        public bool TargetBlank { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
    }
}

