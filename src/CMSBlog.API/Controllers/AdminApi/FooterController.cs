using AutoMapper;
using CMSBlog.Core.Domain.Info;
using CMSBlog.Core.Models.Content;
using CMSBlog.Core.SeedWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CMSBlog.Core.SeedWorks.Constants.Permissions;

namespace CMSBlog.API.Controllers.AdminApi
{
    [Route("api/admin/footer")]
    [ApiController]
    public class FooterController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FooterController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Footer settings

        [HttpGet("settings")]
        [Authorize(Roles.View)]
        public async Task<ActionResult<List<FooterSettingsDto>>> GetSettings()
        {
            var entities = await _unitOfWork.Footer.GetAllAsync();
            var model = _mapper.Map<List<FooterSettingsDto>>(entities);
            return Ok(model);
        }

        [HttpGet("settings/{id}")]
        [Authorize(Roles.View)]
        public async Task<ActionResult<FooterSettingsDto>> GetSettingsById(Guid id)
        {
            var entity = await _unitOfWork.Footer.GetByIdAsync(id);
            if (entity == null) return NotFound();
            var model = _mapper.Map<FooterSettingsDto>(entity);
            return Ok(model);
        }

        [HttpPost("settings")]
        [Authorize(Roles.Edit)]
        public async Task<IActionResult> CreateSettings([FromBody] CreateUpdateFooterSettingsRequest request)
        {
            var entity = _mapper.Map<CreateUpdateFooterSettingsRequest, FooterSettings>(request);
            entity.Id = Guid.NewGuid();
            _unitOfWork.Footer.Add(entity);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPut("settings/{id}")]
        [Authorize(Roles.Edit)]
        public async Task<IActionResult> UpdateSettings(Guid id, [FromBody] CreateUpdateFooterSettingsRequest request)
        {
            var entity = await _unitOfWork.Footer.GetByIdAsync(id);
            if (entity == null) return NotFound();

            _mapper.Map(request, entity);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpDelete("settings")]
        [Authorize(Roles.Delete)]
        public async Task<IActionResult> DeleteSettings([FromQuery] Guid[] ids)
        {
            if (ids == null || ids.Length == 0) return BadRequest();

            foreach (var id in ids)
            {
                var entity = await _unitOfWork.Footer.GetByIdAsync(id);
                if (entity == null) return NotFound();
                _unitOfWork.Footer.Remove(entity);
            }

            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        #endregion

        #region Footer links

        [HttpGet("links")]
        [Authorize(Roles.View)]
        public async Task<ActionResult<List<FooterLinkDto>>> GetLinks()
        {
            var entities = await _unitOfWork.Footer.GetActiveLinksAsync();
            var model = _mapper.Map<List<FooterLinkDto>>(entities);
            return Ok(model);
        }

        [HttpPost("links")]
        [Authorize(Roles.Edit)]
        public async Task<IActionResult> CreateLink([FromBody] CreateUpdateFooterLinkRequest request)
        {
            var entity = _mapper.Map<CreateUpdateFooterLinkRequest, FooterLink>(request);
            entity.Id = Guid.NewGuid();
            _unitOfWork.Footer.AddLink(entity);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPut("links/{id}")]
        [Authorize(Roles.Edit)]
        public async Task<IActionResult> UpdateLink(Guid id, [FromBody] CreateUpdateFooterLinkRequest request)
        {
            var link = await _unitOfWork.Footer.GetLinkByIdAsync(id);
            if (link == null) return NotFound();

            _mapper.Map(request, link);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpDelete("links")]
        [Authorize(Roles.Delete)]
        public async Task<IActionResult> DeleteLinks([FromQuery] Guid[] ids)
        {
            if (ids == null || ids.Length == 0) return BadRequest();

            foreach (var id in ids)
            {
                var link = await _unitOfWork.Footer.GetLinkByIdAsync(id);
                if (link == null) return NotFound();
                _unitOfWork.Footer.RemoveLink(link);
            }

            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        #endregion
    }
}

