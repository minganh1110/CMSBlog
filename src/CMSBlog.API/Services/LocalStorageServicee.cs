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

        public LocalStorageServicee(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _baseUrl = config["AppSettings:BaseUrl"]?.TrimEnd('/') ?? "";
        }

        // Core interface dùng byte[]
        public async Task<string> SaveFileAsync(byte[] content, string fileName, CancellationToken ct = default)
        {
            var baseFolder = @"C:\Users\dell\source\repo_dotnet\CMSBlog\MediaLibrary"; // Thư mục local
            var folder = Path.Combine(baseFolder, "uploads", DateTime.UtcNow.ToString("yyyyMMdd"));
            Directory.CreateDirectory(folder);

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(folder, uniqueFileName);

            await File.WriteAllBytesAsync(filePath, content, ct);

            return $"uploads/{DateTime.UtcNow:yyyyMMdd}/{uniqueFileName}";

        }

        public string GetPublicBaseUrl() => _baseUrl;
    }
}
