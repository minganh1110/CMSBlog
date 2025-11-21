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
    }
}
