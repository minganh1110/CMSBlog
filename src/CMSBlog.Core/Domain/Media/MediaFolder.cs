using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSBlog.Core.Domain.Media
{
    public class MediaFolder
    {
        public Guid Id { get; set; }
        public string FolderName { get; set; } = null!;
        public string SlugName { get; set; } = null!;
        //public string FullPath { get; set; } = null!;
        public Guid? ParentFolderId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid ModifiedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public int PathId { get; set; }                // Id đường dẫn trong bảng Path
        public string? Path { get; set; }         // Đường dẫn đầy đủ của folder

        //navigation properties
        public MediaFolder? ParentFolder { get; set; }     // Folder cha
        public ICollection<MediaFolder>? ChildFolders { get; set; }  // Danh sách folder con
        public ICollection<MediaFileFolderLink>? FileFolderLinks { get; set; }      // Danh sách file trong folder
    }
}
