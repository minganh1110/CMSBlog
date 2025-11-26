using CMSBlog.Core.Application.Interfaces.Media;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CMSBlog.API.Services
{
    public class LocalStorageServicee : IStorageServicee
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _baseUrl;
        private const string _baseFolder = @"MediaLibrary";

        public LocalStorageServicee(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _baseUrl = config["AppSettings:BaseUrl"]?.TrimEnd('/') ?? "";
        }

        // =====================
        // FILE
        // =====================
        // Core interface dùng byte[]
        public async Task<string> SaveFileAsync(byte[] content, string fileName, CancellationToken ct = default)
        {
            var baseFolder = @"MediaLibrary"; // Thư mục local
            var folder = Path.Combine(baseFolder, "uploads");
            Directory.CreateDirectory(folder);

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(folder, uniqueFileName);

            await File.WriteAllBytesAsync(filePath, content, ct);

            return $"uploads/{uniqueFileName}";

        }

        // =====================
        // FOLDER
        // =====================
        public Task CreateFolderAsync(string folderName, string? currentFolder = null, CancellationToken ct = default)
        {
            var parentPath = BuildPath(currentFolder);
            var newFolderPath = Path.Combine(parentPath, folderName);

            if (!Directory.Exists(newFolderPath))
            {
                Directory.CreateDirectory(newFolderPath);
            }

            return Task.CompletedTask;
        }

        public Task RenameFolderAsync(string oldFolderName, string newFolderName, string? currentFolder = null, CancellationToken ct = default)
        {
            var parentPath = BuildPath(currentFolder);

            var oldPath = Path.Combine(parentPath, oldFolderName);
            var newPath = Path.Combine(parentPath, newFolderName);

            if (!Directory.Exists(oldPath))
                throw new DirectoryNotFoundException("Folder không tồn tại");

            if (Directory.Exists(newPath))
                throw new IOException("Folder mới đã tồn tại");

            Directory.Move(oldPath, newPath);

            return Task.CompletedTask;
        }

        public Task DeleteFolderAsync(string folderName, string? currentFolder = null, CancellationToken ct = default)
        {
            var parentPath = BuildPath(currentFolder);
            var deletePath = Path.Combine(parentPath, folderName);

            if (Directory.Exists(deletePath))
            {
                Directory.Delete(deletePath, true); // true = xóa cả con
            }

            return Task.CompletedTask;
        }

        public string GetPublicBaseUrl() => _baseUrl;

        //thiet lap duong dan goc luu tru
        private string BuildPath(string? currentFolder)
        {
            if (string.IsNullOrWhiteSpace(currentFolder))
                return _baseFolder;

            return Path.Combine(_baseFolder, currentFolder);
        }

        private string BuildRelativePath(string? currentFolder, string fileName)
        {
            if (string.IsNullOrWhiteSpace(currentFolder))
                return fileName;

            return Path.Combine(currentFolder, fileName).Replace("\\", "/");
        }
    }
}
