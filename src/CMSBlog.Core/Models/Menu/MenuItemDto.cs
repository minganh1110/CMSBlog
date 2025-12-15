using AutoMapper;
using CMSBlog.Core.Domain.Menu;

namespace CMSBlog.Core.Models.Menu
{
    public class MenuItemDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid? ParentId { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public string MenuGroup { get; set; } = "Main";
        public string LinkType { get; set; }
        public Guid? EntityId { get; set; }
        public string? CustomUrl { get; set; }
        public bool OpenInNewTab { get; set; }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<MenuItem, MenuItemDto>();
            }
        }
    }
}
