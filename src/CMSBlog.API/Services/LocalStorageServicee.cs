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

        public LocalStorageServicee(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            var relativePath = config["Storage:Local:RootPath"] ?? "MediaLibrary/uploads";
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

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(_rootPath, uniqueFileName);

            await File.WriteAllBytesAsync(filePath, content, ct);

            return $"{_baseUrl}/{uniqueFileName}";

        }

        public string GetPublicBaseUrl() => _baseUrl;

        public string ProviderName => "Local";

    }
}
