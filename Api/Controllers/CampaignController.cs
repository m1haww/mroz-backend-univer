using System.Security.Claims;
using BusinessLogic.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mroz.Controllers;

[ApiController]
[Route("api/campaigns")]
[Authorize]
public class CampaignController : ControllerBase
{
    private readonly ICampaignService _campaignService;

    public CampaignController(ICampaignService campaignService)
    {
        _campaignService = campaignService;
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
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var campaigns = await _campaignService.GetAllAsync(userId.Value, ct);
        return Ok(campaigns);
    }

    [HttpGet("{campaignId:long}")]
    public async Task<IActionResult> GetById(long campaignId, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var campaign = await _campaignService.GetByIdAsync(campaignId, userId.Value, ct);
        if (campaign == null) return NotFound(new { message = "Campaign not found." });

        return Ok(campaign);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCampaignDto dto, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var campaign = await _campaignService.CreateAsync(dto, userId.Value, ct);
        if (campaign == null)
            return BadRequest(new { message = "Failed to create campaign. Check Apple Search Ads credentials and request body." });

        return Ok(campaign);
    }

    [HttpPut("{campaignId:long}")]
    public async Task<IActionResult> Update(long campaignId, [FromBody] UpdateCampaignDto dto, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var campaign = await _campaignService.UpdateAsync(campaignId, dto, userId.Value, ct);
        if (campaign == null) return NotFound(new { message = "Campaign not found or update failed." });

        return Ok(campaign);
    }

    [HttpDelete("{campaignId:long}")]
    public async Task<IActionResult> Delete(long campaignId, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var deleted = await _campaignService.DeleteAsync(campaignId, userId.Value, ct);
        if (!deleted) return NotFound(new { message = "Campaign not found or delete failed." });

        return NoContent();
    }
}
