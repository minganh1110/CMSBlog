namespace CMSBlog.API.Services
{
    public class LocalStorageService : IStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _baseUrl;

        public LocalStorageService(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _baseUrl = config["AppSettings:BaseUrl"]?.TrimEnd('/') ?? "";
        }

        public async Task<string> SaveFileAsync(IFormFile file, string fileName, CancellationToken ct = default)
        {
            var folder = Path.Combine(_env.WebRootPath, "uploads", DateTime.UtcNow.ToString("yyyyMMdd"));
            Directory.CreateDirectory(folder);
            var filePath = Path.Combine(folder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream, ct);
            // return relative path from wwwroot
            var relative = $"uploads/{DateTime.UtcNow:yyyyMMdd}/{fileName}";
            return relative;
        }

        public Task DeleteFileAsync(string relativePath)
        {
            var path = Path.Combine(_env.WebRootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(path)) File.Delete(path);
            return Task.CompletedTask;
        }

        public string GetPublicBaseUrl() => _baseUrl; // e.g. https://myhost.com
    }

}
