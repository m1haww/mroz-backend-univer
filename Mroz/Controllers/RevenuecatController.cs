using BusinessLogic.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Mroz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RevenuecatController : ControllerBase
{
    private readonly IRevenuecatUserService _revenuecatUserService;

    public RevenuecatController(IRevenuecatUserService revenuecatUserService)
    {
        _revenuecatUserService = revenuecatUserService;
    }

    [HttpPost("set-user")]
    public async Task<IActionResult> SetUser([FromBody] AddRevenuecatUserDto user, CancellationToken ct)
    {
        var appUser = await _revenuecatUserService.SetUserAsync(user, ct);

        if (user?.Event?.Environment != "PRODUCTION")
        {
            return BadRequest(new { message = $"This is not a production event." });
        }

        if (appUser == null)
            return BadRequest(new { message = "Invalid request; app_user_id is required." });

        return Ok(new { id = appUser.Id, app_user_id = appUser.AppId });
    }
}
