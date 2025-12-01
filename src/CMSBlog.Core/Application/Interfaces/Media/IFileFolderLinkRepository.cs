using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.Interfaces.Media
{
    public interface IFileFolderLinkRepository
    {
        Task AddLinkAsync(Guid fileId, Guid folderId);
        Task RemoveLinkAsync(Guid fileId, Guid folderId);

        Task<List<Guid>> GetFileIdsInFolderAsync(Guid folderId);
        Task<List<Guid>> GetFolderIdsOfFileAsync(Guid fileId);

        Task DeleteLinksByFolderAsync(Guid folderId);
        Task DeleteLinksByFileAsync(Guid fileId);
        Task DeleteByFolderIdsAsync(List<Guid> folderIds);
    }
}
