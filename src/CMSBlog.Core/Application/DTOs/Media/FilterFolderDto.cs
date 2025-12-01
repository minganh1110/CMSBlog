using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.DTOs.Media
{
    public class FilterFolderDto
    {
        public Guid? ParentId { get; set; }
        public string? Search { get; set; }

        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }

        public DateTime? ModifiedFrom { get; set; }
        public DateTime? ModifiedTo { get; set; }

        public string? SortBy { get; set; } = "name"; // name | created | modified | path
        public string? SortDir { get; set; } = "asc"; // asc | desc
    }

}
