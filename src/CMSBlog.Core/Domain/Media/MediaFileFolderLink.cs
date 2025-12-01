using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Domain.Media
{
    public class MediaFileFolderLink
    {
        public int Id { get; set; }
        public Guid MediaFileId { get; set; }
        public Guid MediaFolderId { get; set; }

        public MediaFile MediaFile { get; set; } = null!;
        public MediaFolder MediaFolder { get; set; } = null!;
    }
}
