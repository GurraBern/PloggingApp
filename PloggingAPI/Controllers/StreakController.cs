using Microsoft.AspNetCore.Mvc;
using Plogging.Core.Models;
using PloggingAPI.Services.Interfaces;

namespace PloggingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StreakController : ControllerBase
{
	private readonly IStreakService _streakService;

	public StreakController(IStreakService streakService)
	{
		_streakService = streakService;
	}

    [HttpPut("UpdateStreak")]
    public async Task<ActionResult> UpdateStreak(string userId)
    {
        try
        {
            await _streakService.UpdateStreak(userId);
            return Ok();

        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPut("ResetStreak")]
    public async Task<ActionResult> ResetStreak(string userId)
    {
        try
        {
            await _streakService.ResetStreak(userId);
            return Ok();

        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("GetUserStreak")]
    public async Task<ActionResult<UserStreak>> GetUserStreak(string userId)
    {
        try
        {
            var user = await _streakService.GetUserStreak(userId);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult> CreateUser(UserStreak user)
    {
        try
        {
            await _streakService.CreateUser(user);
            return Ok();

        } catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
        
    }
}


