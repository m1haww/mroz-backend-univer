using System.Security.Claims;
using BusinessLogic.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mroz.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IKeywordService _keywordService;
    private readonly IReportsService _reportsService;

    public ReportsController(IKeywordService keywordService, IReportsService reportsService)
    {
        _keywordService = keywordService;
        _reportsService = reportsService;
    }

    private Guid? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var id) ? id : null;
    }

    [HttpPost("keywords/{campaignId:long}")]
    public async Task<IActionResult> GetKeywordReport(long campaignId, [FromBody] KeywordReportRequestDto request, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var report = await _keywordService.GetKeywordReportAsync(campaignId, userId.Value, request, ct);
        if (report == null)
            return BadRequest(new { message = "Failed to fetch keyword report. Check Apple Search Ads credentials and request body." });

        return Ok(report);
    }

    [HttpPost("campaigns")]
    public async Task<IActionResult> GetCampaignReportList([FromBody] CampaignReportRequestDto request, CancellationToken ct = default)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized(new { message = "User is not authenticated." });

        var report = await _reportsService.GetCampaignReportAsync(userId.Value, request, ct);
        if (report == null)
            return BadRequest(new { message = "Failed to fetch campaign report. Check Apple Search Ads credentials and request body." });

        return Ok(report);
    }
}
