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

    [HttpPatch("AddUserToGroup")]
    public async Task<ActionResult> AddUserToGroup(string ownerUserId, string addUserId)
    {
        try
        {
            await _plogTogetherRepository.AddUserToGroup(ownerUserId, addUserId);
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
}

