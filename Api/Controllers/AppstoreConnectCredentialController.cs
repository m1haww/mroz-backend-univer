using System.Security.Claims;
using BusinessLogic.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mroz.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppstoreConnectCredentialController : ControllerBase
{
    private readonly IAppstoreConnectCredentialService _appstoreConnectCredentialService;

    public AppstoreConnectCredentialController(IAppstoreConnectCredentialService appstoreConnectCredentialService)
    {
        _appstoreConnectCredentialService = appstoreConnectCredentialService;
    }

    [HttpGet]
    public async Task<IActionResult> GetStatus(CancellationToken ct)
    {
        var userId = GetCurrentUserId();

        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var result = await _appstoreConnectCredentialService.GetStatus(userId.Value, ct);

        if (!result.Configured)
        {
            return BadRequest(new { message = "No configuration found." });
        }

        return Ok(result);
    }

    private Guid? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var id) ? id : null;
    }

    [HttpPut]
    public async Task<IActionResult> Save([FromBody] SaveAppStoreConnectDto dto, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var isSuccess = await _appstoreConnectCredentialService.Save(dto, userId.Value, ct);

        if (!isSuccess)
            return BadRequest(new { message = "Error while saving." });

        return Ok(new AppStoreConnectStatusResponse { Configured = true });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var isSuccess = await _appstoreConnectCredentialService.Delete(userId.Value, ct);

        if (!isSuccess)
            return BadRequest(new { message = "Error while deleting." });

        return Ok(NoContent());
    }
}
