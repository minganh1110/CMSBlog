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
        private readonly string _rootPath;
        private readonly string relativePath="";

        public LocalStorageServicee(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
             relativePath = config["Storage:Local:RootPath"] ?? "MediaLibrary/uploads";
            _baseUrl = config["Storage:Local:BaseUrl"] ?? "/uploads";

            // Đây là đường dẫn tuyệt đối nằm cạnh API (.exe)
            _rootPath = Path.Combine(env.ContentRootPath, relativePath);
        }

        // =====================
        // FILE
        // =====================
        // Core interface dùng byte[]
        public async Task<string> SaveFileAsync(byte[] content, string fileName, CancellationToken ct = default)
        {
            if (!Directory.Exists(_rootPath))
                Directory.CreateDirectory(_rootPath);

            var uniqueFileName = $"{fileName}";
            var filePath = Path.Combine(_rootPath, uniqueFileName);

            await File.WriteAllBytesAsync(filePath, content, ct);

            return $"{_baseUrl}/{uniqueFileName}";

        }

        // Lấy stream của file từ storage
        public Task<Stream> GetFileStreamAsync(string filePath, CancellationToken ct = default)
        {
            var path = $"{_rootPath}/{filePath}";
            return Task.FromResult<Stream>(new FileStream(path, FileMode.Open, FileAccess.Read));
        }

        public string GetPublicBaseUrl() => _baseUrl;

        public string ProviderName => "Local";

    }
}
