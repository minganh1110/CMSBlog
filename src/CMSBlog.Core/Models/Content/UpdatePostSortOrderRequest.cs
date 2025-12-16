using System;
using System.Collections.Generic;

namespace CMSBlog.Core.Models.Content
{
    public class UpdatePostSortOrderRequest
    {
        public List<Guid> PostIds { get; set; } = new List<Guid>();
    }
}
