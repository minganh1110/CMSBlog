using CMSBlog.API.DTOs;
using CMSBlog.Core.Application.DTOs.Media;
using CMSBlog.Core.Application.Interfaces.Media;
using CMSBlog.Core.Application.Services.Media;
using CMSBlog.Core.Domain.Media;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMSBlog.API.Controllers.MediaAPI
{
    [ApiController]
    [Route("api/media")]
    public class MediaFileController : ControllerBase
    {
        private readonly IMediaFileService _mediaService;

        public MediaFileController(IMediaFileService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _mediaService.GetAllAsync();
            return Ok(items);
        }
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var item = await _mediaService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("folder/{folderId:guid}")]
        public async Task<IActionResult> GetInFolder([FromRoute] Guid folderId)
        {
            var items = await _mediaService.GetInFolderAsync(folderId);
            return Ok(items);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _mediaService.DeleteAsync(id);
            return NoContent();
        }


        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] CreateMediaFileRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file uploaded");

            using var ms = new MemoryStream();
            await request.File.CopyToAsync(ms);

            var mediaType = _mediaService.DetectMediaType(request.File.ContentType);
            // map sang DTO của Core
            var dto = new CreatedMediaFileDto
            {
                FileContent = ms.ToArray(),
                FileName = request.File.FileName,
                FolderId = request.FolderId,
                MimeType = request.File.ContentType,
                MediaType = mediaType
            };

            var result = await _mediaService.UploadAsync(dto);
            return Ok(result);
        }

        [HttpPatch("{id:guid}/move")]
        public async Task<IActionResult> MoveToFolder([FromRoute] Guid id, [FromBody] MoveMediaFileDto dto)
        {
            await _mediaService.MoveToFolderAsync(id, dto.NewFolderId);
            return NoContent();
        }

        [HttpPatch("{id:guid}/update")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] UpdateMediaFileRequest request)
        {
            if( request.File != null && request.File.Length > 0)
            {
                using var ms = new MemoryStream();
                await request.File.CopyToAsync(ms);
                await _mediaService.ReplaceMediaAsync(id, ms.ToArray());
            }

            if(request.FolderId != null)
            {
                await _mediaService.MoveToFolderAsync(id, request.FolderId.Value);
            }

            var dto = new UpdateMediaFileDto
            {

                FileName = request.FileName,
                Description = request.Description,
                AltText = request.AltText,
                Caption = request.Caption
            };
            var result = await _mediaService.UpdateAsync(id, dto);
            return Ok(result);
        }

    }

}
