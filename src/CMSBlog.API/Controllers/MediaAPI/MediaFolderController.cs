using CMSBlog.Core.Application.DTOs.Media;
using CMSBlog.Core.Application.Interfaces.Media;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/folders")]
public class MediaFolderController : ControllerBase
{
    private readonly IMediaFolderService _service;

    public MediaFolderController(IMediaFolderService service)
    {
        _service = service;
    }

    // -------------------------------------------------------------
    // 1) Tạo folder
    // -------------------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMediaFolderDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }

    // -------------------------------------------------------------
    // 2) Lấy cây thư mục
    // -------------------------------------------------------------
    [HttpGet("tree")]
    public async Task<IActionResult> GetTree()
    {
        var result = await _service.GetTreeAsync();
        return Ok(result);
    }

    // -------------------------------------------------------------
    // 3) Filter folders
    // -------------------------------------------------------------
    [HttpGet("filter")]
    public async Task<IActionResult> Filter([FromQuery] FilterFolderDto filter)
    {
        var result = await _service.FilterAsync(filter);
        return Ok(result);
    }

    // -------------------------------------------------------------
    // 4) Rename + Move (Edit)
    // -------------------------------------------------------------
    /// <summary>
    /// Edit folder: có thể rename, move hoặc cả hai cùng lúc
    /// </summary>
    [HttpPatch("{id}/edit")]
    public async Task<IActionResult> Edit(Guid id, [FromBody] UpdateMediaFolderDto dto)
    {
        var ok = await _service.EditAsync(id, dto);
        if (!ok) return NotFound();
        return Ok();
    }

    // -------------------------------------------------------------
    // 5) Move
    // -------------------------------------------------------------
    [HttpPatch("{id}/move")]
    public async Task<IActionResult> Move(Guid id, [FromBody] MoveFolderDto body)
    {
        var ok = await _service.MoveAsync(id, body.NewParentId);
        if (!ok) return NotFound();
        return Ok();
    }

    // -------------------------------------------------------------
    // 6) Delete
    // -------------------------------------------------------------
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _service.DeleteAsync(id);
        if (!ok) return NotFound();
        return Ok();
    }
}
