using AutoMapper;
using CMSBlog.Core.Domain.Menu;
using CMSBlog.Core.Models.Menu;
using CMSBlog.Core.SeedWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CMSBlog.Core.SeedWorks.Constants.Permissions;

namespace CMSBlog.API.Controllers.AdminApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItems()
        {
            var items = await _unitOfWork.Menu.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<MenuItemDto>>(items);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        
        public async Task<ActionResult<MenuItemDto>> GetMenuItem(Guid id)
        {
            var item = await _unitOfWork.Menu.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<MenuItemDto>(item);
            return Ok(dto);
        }

        [HttpPost]
        
        public async Task<ActionResult<MenuItemDto>> CreateMenuItem(CreateMenuItemRequest request)
        {
            try
            {
                var item = _mapper.Map<MenuItem>(request);
                item.Id = Guid.NewGuid();
            
                _unitOfWork.Menu.Add(item);
                await _unitOfWork.CompleteAsync();

                var dto = _mapper.Map<MenuItemDto>(item);
                // Changing CreatedAtAction to Ok to avoid potential route generation errors
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        

        public async Task<ActionResult> UpdateMenuItem(Guid id, UpdateMenuItemRequest request)
        {
            var item = await _unitOfWork.Menu.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _mapper.Map(request, item);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMenuItem(Guid id)
        {
            var item = await _unitOfWork.Menu.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _unitOfWork.Menu.Remove(item);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
