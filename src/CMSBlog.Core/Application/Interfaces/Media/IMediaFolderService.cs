using CMSBlog.Core.Application.DTOs.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.Interfaces.Media
{
    public interface IMediaFolderService
    {
        Task<MediaFolderDto> CreateAsync(CreateMediaFolderDto dto, Guid? userId = null);
        Task<MediaFolderDto?> GetByIdAsync(Guid id);
        Task<MediaFolderDto?> GetByIdIncludeFilesAsync(Guid id);
        Task<List<MediaFolderDto>> GetTreeAsync();
        Task<bool> RenameAsync(Guid id, string newName);
        Task<bool> MoveAsync(Guid id, Guid? newParentId);
        Task<bool> EditAsync(Guid id, UpdateMediaFolderDto dto);
        Task<bool> DeleteAsync(Guid id, bool removeFileLinks = true);
        //Task<string> BuildFolderPathAsync(Guid? folderId); // returns slug path like "root/child"
        Task<List<MediaFolderDto>> FilterAsync(FilterFolderDto filter);
    }

}
