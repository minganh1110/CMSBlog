using CMSBlog.API.DTOs;
using CMSBlog.Core.Application.DTOs.Media;
using CMSBlog.Core.Application.Interfaces.Media;
using CMSBlog.Core.Application.Services.Media;
using Microsoft.AspNetCore.Mvc;

namespace CMSBlog.API.Controllers.SettingAPI
{
    [ApiController]
    [Route("api/provider")]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _service;

        public ProviderController(IProviderService service)
        {
            _service = service;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent()
        {
            var provider = await _service.GetCurrentProviderAsync();
            return Ok(new { provider });
        }

        [HttpPost("change")]
        public async Task<IActionResult> ChangeProvider([FromBody] ChangeProviderRequest request)
        {
            await _service.ChangeProviderAsync(request.Provider);
            return Ok(new { message = "Provider updated", provider = request.Provider });
        }
    }


}
