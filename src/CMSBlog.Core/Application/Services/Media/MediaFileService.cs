using AutoMapper;
using CMSBlog.Core.Application.DTOs.Media;
using CMSBlog.Core.Application.Interfaces.Media;
using CMSBlog.Core.Domain.Constants;
using CMSBlog.Core.Domain.Media;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CMSBlog.Core.Application.Services.Media
{
    public class MediaFileService : IMediaFileService
    {
        private readonly IMediaFileRepository _repo;
        private readonly IMapper _mapper;
        private readonly IStorageFactory _storageFactory;
        private readonly IMediaFolderRepository _folderRepo;
        private readonly IFileFolderLinkRepository _fileFolderRepo;
        int height;
        int width;
        public async Task<string> UploadAsync(byte[] content, string fileName)
        {
            var provider = await _storageFactory.GetStorageServiceAsync();
            return await provider.SaveFileAsync(content, fileName);
        }

        public MediaFileService(IMediaFileRepository repo, IMapper mapper, IStorageFactory storageFactory, 
            IMediaFolderRepository folderrepo, IFileFolderLinkRepository linkrepo)
        {
            _repo = repo;
            _mapper = mapper; 
            _storageFactory = storageFactory;
            _folderRepo = folderrepo;
            _fileFolderRepo = linkrepo;
        }
        
        public async Task<MediaFileDto> UploadAsync(CreatedMediaFileDto dto, CancellationToken ct = default)
        {
            if (dto.FileContent is null || dto.FileContent.Length == 0)
                throw new ArgumentException("File is empty");

            // 1. Slug + file name
            var slug = GenerateSlug(Path.GetFileNameWithoutExtension(dto.FileName));
            var ext = Path.GetExtension(dto.FileName);
            var fileName = $"{slug}{ext}";
            

            // 2. Lưu file qua provider
            var storage = await _storageFactory.GetStorageServiceAsync();
            var storagePath = await storage.SaveFileAsync(dto.FileContent, fileName, ct);

            // 3. Xác định folderId (nếu null thì gán ROOT)
            Guid folderId = dto.FolderId ?? MediaFolderConstants.RootFolderId;

            // Kiểm tra folder có tồn tại không
            var folder = await _folderRepo.GetByIdAsync(folderId)
                        ?? throw new Exception("Folder does not exist");

            // lấy chiều cao từ dto nếu có
            if ( dto.MediaType == MediaType.Image)
            {
                // Cố gắng đọc chiều cao từ ảnh
                using var image = Image.Load(dto.FileContent);
                height = image.Height;
                width = image.Width;
            }

            // 4. Tạo formats: thumbnail, small, medium, large

            var imagee = await storage.GetFileStreamAsync(fileName, ct);
            var original = await ToMemoryStreamAsync(imagee);
            var formats = await GenerateAllFormats(original,fileName, ext);
            var mediaFormats = new MediaFormats
            {
                Thumbnail = formats["thumbnail"],
                Small = formats["small"],
                Medium = formats["medium"],
                Large = formats["large"]
            };


            // 4. Tạo MediaFile
            var entity = new MediaFile
            {
                ID = Guid.NewGuid(),
                FileName = dto.FileName,
                SlugName = slug,
                FileExtension = ext,
                FilePath = storagePath,
                FileSize = dto.FileContent.Length,
                MimeType = dto.MimeType,
                DateCreated = DateTime.UtcNow,
                Provider = storage.ProviderName,
                MediaType = dto.MediaType,
                Height = height,
                Width = width,
                Formats = mediaFormats

            };
            

            await _repo.AddAsync(entity);

            // 5. Tạo liên kết qua bảng trung gian MediaFileFolder
            await _fileFolderRepo.AddLinkAsync(entity.ID, folderId);

            // 6. Trả về DTO
            var result = _mapper.Map<MediaFileDto>(entity);
            result.FileUrl = $"{storagePath}";
            result.FolderId = folderId;

            return result;
        }

        public async Task<MediaFormat> GenerateFormatAsync(Stream original, string NameFile,
            int targetWidth, string prefix, string ext, CancellationToken ct = default)
        {
            original.Position = 0;

            using (var image = Image.Load(original))
            {
                var ratio = (double)targetWidth / image.Width;
                var newHeight = (int)(image.Height * ratio);

                image.Mutate(x => x.Resize(targetWidth, newHeight));

                // 🟦 1. Lưu image vào MemoryStream thay vì Disk
                using var ms = new MemoryStream();
                await image.SaveAsync(ms, GetEncoder(ext));

                string fileName = $"{prefix}_{NameFile}";

                // 2. Lưu file qua provider
                ms.Position = 0;
                var content = ms.ToArray();
                var storage = await _storageFactory.GetStorageServiceAsync();
                var storagePath = await storage.SaveFileAsync(content, fileName, ct);

                return new MediaFormat
                {
                    Name = fileName,
                    Ext = ext,
                    Mime = "image/" + ext.Trim('.'),
                    Url = storagePath,
                    Width = targetWidth,
                    Height = newHeight,
                    Size = Math.Round(content.Length / 1024.0, 2),
                    SizeInBytes = content.Length
                };
            }
        }

        public async Task<Dictionary<string, MediaFormat>> GenerateAllFormats(Stream original,string NameFile, string ext)
        {
            var formats = new Dictionary<string, MediaFormat>();

            formats["thumbnail"] = await GenerateFormatAsync(original,NameFile, 245, "thumbnail", ext);
            formats["small"] = await GenerateFormatAsync(original,NameFile, 500, "small", ext);
            formats["medium"] = await GenerateFormatAsync(original,NameFile, 750, "medium", ext);
            formats["large"] = await GenerateFormatAsync(original,NameFile, 1000, "large", ext);

            return formats;
        }



        public async Task<List<MediaFileDto>> GetAllAsync()
        {
            var storage = await _storageFactory.GetStorageServiceAsync();
            var providerName = storage.ProviderName;

            var items = await _repo.GetAllAsync(providerName);

            var baseUrl = storage.GetPublicBaseUrl();
            var result = _mapper.Map<List<MediaFileDto>>(items);

            foreach (var file in result)
            {
                if (file.Formats != null)
                {
                    //lấy full url cho file chính
                    file.FileUrl = $"{baseUrl}{file.FilePath}".Replace("\\", "/");
                    //lấy full url cho từng format
                    if (file.Formats != null)
                    {
                        foreach (var fmt in new[] { file.Formats.Thumbnail, file.Formats.Small,
                                    file.Formats.Medium, file.Formats.Large })
                        {
                            if (fmt != null)
                                fmt.Url = $"{baseUrl}{fmt.Url}".Replace("\\", "/");
                        }

                    }

                }
            }
                

            return result;
        }


        public async Task<List<MediaFileDto>> GetInFolderAsync(Guid folderId)
        {
            var storage = await _storageFactory.GetStorageServiceAsync();
            var providerName = storage.ProviderName;

            var items = await _repo.GetByFolderIdAsync(folderId, providerName);

            var baseUrl = storage.GetPublicBaseUrl();
            var result = _mapper.Map<List<MediaFileDto>>(items);

            foreach (var file in result)
            {
                if (file.Formats != null)
                {
                    //lấy full url cho file chính
                    file.FileUrl = $"{baseUrl}{file.FilePath}".Replace("\\", "/");
                    //lấy full url cho từng format
                    if (file.Formats != null)
                    {
                        foreach (var fmt in new[] { file.Formats.Thumbnail, file.Formats.Small,
                                    file.Formats.Medium, file.Formats.Large })
                        {
                            if (fmt != null)
                                fmt.Url = $"{baseUrl}{fmt.Url}".Replace("\\", "/");
                        }

                    }

                }
            }
                

            return result;
        }

        public async Task<MediaFileDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            var storage = await _storageFactory.GetStorageServiceAsync();
            var baseUrl = storage.GetPublicBaseUrl();
            var dto = _mapper.Map<MediaFileDto>(entity);
            dto.FileUrl = $"{baseUrl}{dto.FilePath}".Replace("\\","/");
            if (dto.Formats != null)
            {
                foreach (var fmt in new[] { dto.Formats.Thumbnail, dto.Formats.Small,
                                dto.Formats.Medium, dto.Formats.Large })
                {
                    if (fmt != null)
                        fmt.Url = $"{baseUrl}{fmt.Url}".Replace("\\","/");
                }
            }

            return dto;
        }


        public async Task<bool> UpdateAsync(Guid id, UpdateMediaFileDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            // Map DTO → entity (partial update)
            _mapper.Map(dto, entity);

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> ReplaceMediaAsync(Guid id, byte[] newContent, CancellationToken ct = default)
        {
            var height = 0;
            var width = 0;
            if (newContent is null || newContent.Length == 0)
                throw new ArgumentException("File is empty");
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            // Lưu file mới qua provider
            var storage = await _storageFactory.GetStorageServiceAsync();
            var fileName = GetKeyFromPathOrUrl(entity.FilePath);
            // lấy chiều cao từ dto nếu có
            if (entity.MediaType == MediaType.Image)
            {
                // Cố gắng đọc chiều cao từ ảnh
                using var image = Image.Load(newContent);
                    height = image.Height;
                    width = image.Width;    
            }
            

            var storagePath = await storage.SaveFileAsync(newContent, fileName, ct);

            // ReGenerate formats: thumbnail, small, medium, large
            var imagee = await storage.GetFileStreamAsync(fileName, ct);
            var original = await ToMemoryStreamAsync(imagee);
            var formats = await GenerateAllFormats(original, fileName, entity.FileExtension);
            var mediaFormats = new MediaFormats
            {
                Thumbnail = formats["thumbnail"],
                Small = formats["small"],
                Medium = formats["medium"],
                Large = formats["large"]
            };
            entity.Formats = mediaFormats;
            // Cập nhật thông tin file trong entity
            entity.FileSize = newContent.Length;
            entity.Height= height;
            entity.Width= width;
            entity.DateModified = DateTime.UtcNow;
            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> MoveToFolderAsync(Guid fileId, Guid newFolderId)
        {
            var file = await _repo.GetByIdAsync(fileId);
            if (file == null) return false;
            var newFolder = await _folderRepo.GetByIdAsync(newFolderId);
            if (newFolder == null) return false;
            // Xóa liên kết hiện tại
            await _fileFolderRepo.DeleteLinksByFileAsync(fileId);
            // Tạo liên kết mới
            await _fileFolderRepo.AddLinkAsync(fileId, newFolderId);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
            return true;
        }

        private string GenerateSlug(string input)
        {
            var noDiacritics = input.Normalize(NormalizationForm.FormD);
            var chars = noDiacritics.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            var cleaned = new string(chars).Normalize(NormalizationForm.FormC);

            cleaned = cleaned.ToLower().Replace(" ", "-");

            return cleaned;
        }

        public MediaType DetectMediaType(string mime)
        {
            if (string.IsNullOrWhiteSpace(mime))
                return MediaType.Other;

            if (mime.StartsWith("image/"))
                return MediaType.Image;

            if (mime.StartsWith("video/"))
                return MediaType.Video;

            if (mime.StartsWith("audio/"))
                return MediaType.Audio;

            if (mime == "application/pdf")
                return MediaType.Document;

            if (mime.Contains("excel") || mime.Contains("spreadsheet"))
                return MediaType.Document;

            if (mime.Contains("word"))
                return MediaType.Document;

            return MediaType.Other;
        }

        private IImageEncoder GetEncoder(string ext)
        {
            return ext.ToLower() switch
            {
                ".png" => new PngEncoder(),
                ".jpg" or ".jpeg" => new JpegEncoder(),
                ".webp" => new WebpEncoder(),
                _ => new PngEncoder()
            };
        }

        private async Task<MemoryStream> ToMemoryStreamAsync(Stream input)
        {
            var ms = new MemoryStream();
            await input.CopyToAsync(ms);
            ms.Position = 0;
            return ms;
        }

        private static string GetKeyFromPathOrUrl(string pathOrUrl)
        {
            if (string.IsNullOrEmpty(pathOrUrl)) return Guid.NewGuid().ToString("N");

            if (Uri.IsWellFormedUriString(pathOrUrl, UriKind.Absolute))
            {
                var uri = new Uri(pathOrUrl);
                // AbsolutePath like /bucket/key/file.png -> take last segment or whole path as key depending on your S3 mapping
                // Here we take last segment to keep filename (if your S3 uses folder keys, adjust accordingly)
                return Path.GetFileName(uri.AbsolutePath);
            }
            // otherwise assume it's already a key or filepath; if it's a full local path, take filename
            return Path.GetFileName(pathOrUrl);
        }

        public static string NormalizeSlug(string filename)
        {
            var noDiacritics = filename.Normalize(NormalizationForm.FormD);
            var chars = noDiacritics.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            var cleaned = new string(chars).Normalize(NormalizationForm.FormC);

            cleaned = cleaned.ToLower().Replace(" ", "-");

            return cleaned;
        }

    }
}
