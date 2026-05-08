using System.Security.Claims;
using BusinessLogic.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mroz.Controllers;

[ApiController]
[Route("api/campaigns/{campaignId:long}/adgroups")]
[Authorize]
public class AdGroupController : ControllerBase
{
    private readonly IAdGroupService _adGroupService;

    public AdGroupController(IAdGroupService adGroupService)
    {
        _adGroupService = adGroupService;
    }

    private Guid? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var id) ? id : null;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(long campaignId, [FromQuery] int? limit, [FromQuery] int? offset, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var adGroups = await _adGroupService.GetAllAsync(campaignId, userId.Value, limit, offset, ct);
        return Ok(adGroups);
    }

    [HttpGet("{adGroupId:long}")]
    public async Task<IActionResult> GetById(long campaignId, long adGroupId, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var adGroup = await _adGroupService.GetByIdAsync(campaignId, adGroupId, userId.Value, ct);
        if (adGroup == null) return NotFound(new { message = "Ad group not found." });

        return Ok(adGroup);
    }

    [HttpPost]
    public async Task<IActionResult> Create(long campaignId, [FromBody] CreateAdGroupDto dto, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var adGroup = await _adGroupService.CreateAsync(campaignId, dto, userId.Value, ct);
        if (adGroup == null)
            return BadRequest(new { message = "Failed to create ad group. Check Apple Search Ads credentials and request body." });

        return Ok(adGroup);
    }

    [HttpPut("{adGroupId:long}")]
    public async Task<IActionResult> Update(long campaignId, long adGroupId, [FromBody] UpdateAdGroupDto dto, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var adGroup = await _adGroupService.UpdateAsync(campaignId, adGroupId, dto, userId.Value, ct);
        if (adGroup == null)
            return NotFound(new { message = "Ad group not found or update failed." });

        return Ok(adGroup);
    }

    [HttpDelete("{adGroupId:long}")]
    public async Task<IActionResult> Delete(long campaignId, long adGroupId, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var deleted = await _adGroupService.DeleteAsync(campaignId, adGroupId, userId.Value, ct);
        if (!deleted) return NotFound(new { message = "Ad group not found or delete failed." });

        return NoContent();
    }
}
