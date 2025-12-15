using AutoMapper;
using CMSBlog.Core.Domain.Menu;
using System.ComponentModel.DataAnnotations;

namespace CMSBlog.Core.Models.Menu
{
    public class CreateMenuItemRequest
    {
        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }

        public Guid? ParentId { get; set; }

        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;

        [Required]
        [MaxLength(50)]
        public string MenuGroup { get; set; } = "Main";

        [Required]
        [MaxLength(50)]
        public required string LinkType { get; set; }

        public Guid? EntityId { get; set; }

        [MaxLength(500)]
        public string? CustomUrl { get; set; }

        public bool OpenInNewTab { get; set; } = false;

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<CreateMenuItemRequest, MenuItem>();
            }
        }
    }
}
