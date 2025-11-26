using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSBlog.Core.Domain.Media
{
    public class MediaFolder
    {
        public Guid ID { get; set; }
        public string FolderName { get; set; } = null!;
        public string SlugName { get; set; } = null!;
        public string FullPath { get; set; } = null!;
        public Guid? ParentFolderId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public MediaFolder? ParentFolder { get; set; }     // Folder cha
        public ICollection<MediaFolder>? ChildFolders { get; set; }  // Danh sách folder con
        public ICollection<MediaFile>? MediaFiles { get; set; }      // Danh sách file trong folder
    }
}
