using CMSBlog.API.DTOs;
using CMSBlog.Core.Application.DTOs.Media;
using CMSBlog.Core.Application.Interfaces.Media;
using CMSBlog.Core.Application.Services.Media;
using Microsoft.AspNetCore.Mvc;

namespace CMSBlog.API.Controllers.MediaAPI
{
    [ApiController]
    [Route("api/media")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaFileService _mediaService;

        public MediaController(IMediaFileService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _mediaService.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var item = await _mediaService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
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

            // map sang DTO của Core
            var dto = new CreatedMediaFileDto
            {
                FileContent = ms.ToArray(),
                FileName = request.File.FileName,
                FolderId = request.FolderId
            };

            var result = await _mediaService.UploadAsync(dto);
            return Ok(result);
        }

    }

}
