using System.Security.Claims;
using BusinessLogic.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mroz.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppleSearchAdsCredentialController : ControllerBase
{
    private readonly IAppleSearchAdsCredentialService _appleSearchAdsCredentialService;

    public AppleSearchAdsCredentialController(IAppleSearchAdsCredentialService appleSearchAdsCredentialService)
    {
        _appleSearchAdsCredentialService = appleSearchAdsCredentialService;
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetStatus(CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var result = await _appleSearchAdsCredentialService.GetStatus(userId.Value, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddAppleSearchAdsDto dto, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var credentialId = await _appleSearchAdsCredentialService.AddCredential(userId.Value, dto, ct);
        if (credentialId == null)
            return BadRequest(new { message = "Invalid or missing credential data." });

        return Ok(new { credentialId });
    }

    [HttpGet("access-token")]
    public async Task<IActionResult> GetAccessToken(CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var result = await _appleSearchAdsCredentialService.GetOrCreateAccessToken(userId.Value, ct);
        if (result == null)
            return BadRequest(new { message = "No Apple Search Ads credential found or token exchange failed." });

        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var deleted = await _appleSearchAdsCredentialService.Delete(userId.Value, ct);
        if (!deleted)
            return NotFound(new { message = "No credential to delete." });

        return NoContent();
    }

    private Guid? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var id) ? id : null;
    }
}
