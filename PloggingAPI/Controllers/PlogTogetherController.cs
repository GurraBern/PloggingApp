using Microsoft.AspNetCore.Mvc;
using PloggingAPI.Repository.Interfaces;

namespace PloggingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlogTogetherController : Controller
{
    private readonly IPlogTogetherRepository _plogTogetherRepository;

    public PlogTogetherController(IPlogTogetherRepository plogTogetherRepository)
    {
        _plogTogetherRepository = plogTogetherRepository;
    }

    [HttpGet("GetPlogTogether/{ownerUserId}")]
    public async Task<ActionResult> GetPlogTogether(string ownerUserId)
    {
        try
        {
            var user = await _plogTogetherRepository.GetPlogTogether(ownerUserId);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost("AddUserToGroup/{ownerUserId}/{userId}")]
    public async Task<ActionResult> AddUserToGroup(string ownerUserId, string userId)
    {
        try
        {
            await _plogTogetherRepository.AddUserToGroup(ownerUserId, userId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpDelete("DeleteGroup")]
    public async Task<ActionResult> DeleteGroup(string ownerUserId)
    {
        try
        {
            await _plogTogetherRepository.DeleteGroup(ownerUserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPut("RemoveUserFromGroup/{ownerUserId}/{userId}")]
    public async Task<ActionResult> RemoveUserFromGroup(string ownerUserId, string userId)
    {
        try
        {
            await _plogTogetherRepository.RemoveUserFromGroup(ownerUserId, userId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}

