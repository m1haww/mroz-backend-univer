using System.Security.Claims;
using BusinessLogic.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mroz.Controllers;

[ApiController]
[Route("api/campaigns/{campaignId:long}/adgroups/{adGroupId:long}/keywords")]
[Authorize]
public class KeywordController : ControllerBase
{
    private readonly IKeywordService _keywordService;

    public KeywordController(IKeywordService keywordService)
    {
        _keywordService = keywordService;
    }

    private Guid? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var id) ? id : null;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(long campaignId, long adGroupId, [FromQuery] int? limit, [FromQuery] int? offset, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var keywords = await _keywordService.GetAllAsync(campaignId, adGroupId, userId.Value, limit, offset, ct);
        return Ok(keywords);
    }

    [HttpGet("{keywordId:long}")]
    public async Task<IActionResult> GetById(long campaignId, long adGroupId, long keywordId, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var keyword = await _keywordService.GetByIdAsync(campaignId, adGroupId, keywordId, userId.Value, ct);
        if (keyword == null) return NotFound(new { message = "Keyword not found." });

        return Ok(keyword);
    }

    [HttpPost]
    public async Task<IActionResult> Create(long campaignId, long adGroupId, [FromBody] CreateKeywordDto dto, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var keyword = await _keywordService.CreateAsync(campaignId, adGroupId, dto, userId.Value, ct);
        if (keyword == null)
            return BadRequest(new { message = "Failed to create keyword. Check Apple Search Ads credentials and request body." });

        return Ok(keyword);
    }

    [HttpPut]
    public async Task<IActionResult> Update(long campaignId, long adGroupId, [FromBody] UpdateKeywordDto dto, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var keyword = await _keywordService.UpdateAsync(campaignId, adGroupId, dto, userId.Value, ct);
        if (keyword == null) return NotFound(new { message = "Keyword not found or update failed." });

        return Ok(keyword);
    }

    [HttpDelete("{keywordId:long}")]
    public async Task<IActionResult> Delete(long campaignId, long adGroupId, long keywordId, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var deleted = await _keywordService.DeleteAsync(campaignId, adGroupId, keywordId, userId.Value, ct);
        if (!deleted) return NotFound(new { message = "Keyword not found or delete failed." });

        return NoContent();
    }
}
