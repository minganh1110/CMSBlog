using CMSBlog.Core.Application.Interfaces.Media;

public class LocalStorageServicee : IStorageServicee
{
    private readonly string _rootFolder;
    private readonly string _requestPath;
    private readonly IHttpContextAccessor _httpContext;

    public LocalStorageServicee(IWebHostEnvironment env, IConfiguration config, IHttpContextAccessor httpContext)
    {
        // Thư mục lưu file bên trong wwwroot
        _rootFolder = Path.Combine(env.ContentRootPath, "MediaLibrary", "uploads");

        // URL public
        _requestPath = "/uploads";

        if (!Directory.Exists(_rootFolder))
            Directory.CreateDirectory(_rootFolder);
        _httpContext = httpContext;
    }

    public async Task<string> SaveFileAsync(byte[] content, string fileName, CancellationToken ct = default)
    {
        var filePath = Path.Combine(_rootFolder, fileName);
        await File.WriteAllBytesAsync(filePath, content, ct);

        // Trả về URL mà UI có thể dùng
        return $"{_requestPath}/{fileName}";
    }

    public Task<Stream> GetFileStreamAsync(string fileUrl, CancellationToken ct = default)
    {
        // fileUrl = /uploads/abc.jpg
        var fileName = Path.GetFileName(fileUrl);
        var fullPath = Path.Combine(_rootFolder, fileName);

        return Task.FromResult<Stream>(new FileStream(fullPath, FileMode.Open, FileAccess.Read));
    }

    public string GetPublicBaseUrl()
    {
        var request = _httpContext.HttpContext?.Request;
        if (request == null)
            return ""; // fallback khi chạy background task

        var baseUrl = $"{request.Scheme}://{request.Host}";
        return baseUrl.Replace("\\", "/");
    }

    public string ProviderName => "Local";
}
