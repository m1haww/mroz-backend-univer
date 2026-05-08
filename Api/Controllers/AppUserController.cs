using BusinessLogic.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mroz.Controllers;

[ApiController]
[Route("api/appusers")]
[Authorize]
public class AppUserController : ControllerBase
{
    private readonly IAppUserService _appUserService;

    public AppUserController(IAppUserService appUserService)
    {
        _appUserService = appUserService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? appId, CancellationToken ct)
    {
        var users = await _appUserService.GetAllAsync(appId, ct);
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var user = await _appUserService.GetByIdAsync(id, ct);
        if (user == null) return NotFound(new { message = "AppUser not found." });
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppUserDto dto, CancellationToken ct)
    {
        var user = await _appUserService.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAppUserDto dto, CancellationToken ct)
    {
        var user = await _appUserService.UpdateAsync(id, dto, ct);
        if (user == null) return NotFound(new { message = "AppUser not found." });
        return Ok(user);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _appUserService.DeleteAsync(id, ct);
        if (!deleted) return NotFound(new { message = "AppUser not found." });
        return NoContent();
    }
}
