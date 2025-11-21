namespace CMSBlog.API.Services
{
    public interface IStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string fileName, CancellationToken ct = default);
        Task DeleteFileAsync(string relativePath);
        string GetPublicBaseUrl(); // used to build Url for clients
    }

}
