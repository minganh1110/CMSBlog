using System.Threading;
using System.Threading.Tasks;

namespace CMSBlog.Core.Application.Interfaces.Media
{
    public interface IStorageServicee
    {
        /// <summary>
        /// Lưu file và trả về đường dẫn lưu trong storage
        /// </summary>
        Task<string> SaveFileAsync(byte[] content, string fileName, CancellationToken ct = default);

        /// <summary>
        /// Trả về base URL public để client có thể truy cập
        /// </summary>
        string GetPublicBaseUrl();

        /// <summary>
        /// Luu thư mục vật lý trong storage
        /// </summary>
        Task CreateFolderAsync(string FolderName, string? currentFolder = null, CancellationToken ct = default);
        /// <summary>
        /// Đổi tên thư mục vật lý trong storage
        /// </summary>
        Task RenameFolderAsync(string oldFolderName, string newFolderName, string? currentFolder = null, CancellationToken ct = default);
        Task DeleteFolderAsync(string folderName, string? currentFolder = null, CancellationToken ct = default);

    }
}
