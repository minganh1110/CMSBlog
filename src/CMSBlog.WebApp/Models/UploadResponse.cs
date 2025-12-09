using System.Text.Json.Serialization;

namespace CMSBlog.WebApp.Models
{
    public class UploadResponse
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }
    }
}
