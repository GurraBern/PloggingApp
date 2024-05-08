using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plogging.Core.Models;
using PloggingAPI.Services.Interfaces;

namespace PloggingAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class LitterBagPlacementController : ControllerBase
{
    private readonly ILitterbagPlacementService _litterbagPlacementService;

    public LitterBagPlacementController(ILitterbagPlacementService litterbagPlacementService)
    {
        _litterbagPlacementService = litterbagPlacementService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateLitterbagPlacement([FromBody] LitterbagPlacement litterbagPlacement)
    {
        try
        {
            await _litterbagPlacementService.CreateLitterbagPlacement(litterbagPlacement);
            return Ok();
                       }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LitterbagPlacement>>> GetLitterbagPlacements()
    {
        try
        {
            var litterBagPlacements = await _litterbagPlacementService.GetLitterbagPlacements();

            return Ok(litterBagPlacements);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> CollectLitterbagPlacement(string litterbagPlacementId, int distanceToLitterbag)
    {
        try
        {
            await _litterbagPlacementService.CollectLitterbagPlacement(litterbagPlacementId, distanceToLitterbag);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
