using CMSBlog.Core.Application.Interfaces.Media;
using Amazon.S3;
using Amazon.S3.Model;

public class S3StorageService : IStorageServicee
{
    private readonly IAmazonS3 _s3Client ;
    private readonly string _bucketName ;
    private readonly string _publicBaseUrl ;

    public S3StorageService(IConfiguration config, IAmazonS3 s3Client)
    {
        _s3Client = s3Client;

        _bucketName = config["Storage:S3:Bucket"]
            ?? throw new ArgumentNullException("Storage:S3:Bucket is required");

        _publicBaseUrl = config["Storage:S3:BaseUrl"]
            ?? throw new ArgumentNullException("Storage:S3:BaseUrl is required");
        // https://mybucket.s3.amazonaws.com
    }

    public async Task<string> SaveFileAsync(byte[] content, string fileName, CancellationToken ct = default)
    {
        using var stream = new MemoryStream(content);

        var upload = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            InputStream = stream,
            ContentType = "application/octet-stream"
        };

        await _s3Client.PutObjectAsync(upload, ct);

        // Trả về URL công khai của file
        return $"{_publicBaseUrl}/{fileName}";
    }

    public string GetPublicBaseUrl() => _publicBaseUrl;

    public string ProviderName => "S3";
}
