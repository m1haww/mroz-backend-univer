using System.Security.Claims;
using BusinessLogic.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mroz.Controllers;

[ApiController]
[Route("api/apps")]
[Authorize]
public class AppController : ControllerBase
{
    private readonly IAppService _appService;

    public AppController(IAppService appService)
    {
        _appService = appService;
    }

    private Guid? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var id) ? id : null;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var apps = await _appService.GetAllAsync(userId.Value, ct);
        return Ok(apps);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var app = await _appService.GetByIdAsync(id, userId.Value, ct);
        if (app == null) return NotFound(new { message = "App not found." });
        return Ok(app);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppDto dto, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var app = await _appService.CreateAsync(dto, userId.Value, ct);
        return CreatedAtAction(nameof(GetById), new { id = app.Id }, app);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateAppDto dto, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var app = await _appService.UpdateAsync(id, dto, userId.Value, ct);
        if (app == null) return NotFound(new { message = "App not found." });
        return Ok(app);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var deleted = await _appService.DeleteAsync(id, userId.Value, ct);
        if (!deleted) return NotFound(new { message = "App not found." });
        return NoContent();
    }
}
