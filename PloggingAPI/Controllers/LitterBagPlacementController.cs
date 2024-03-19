using Microsoft.AspNetCore.Mvc;
using Plogging.Core.Models;
using PloggingAPI.Services.Interfaces;

namespace PloggingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LitterBagPlacementController : ControllerBase
    {
        private readonly ILitterBagPlacementService _litterBagPlacementService;

        public LitterBagPlacementController(ILitterBagPlacementService litterBagPlacementService)
        {
            _litterBagPlacementService = litterBagPlacementService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLitterBagPlacement([FromBody] LitterBagPlacement litterBagPlacement)
        {
            try
            {
                await _litterBagPlacementService.CreateLitterBagPlacement(litterBagPlacement);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LitterBagPlacement>>> GetLitterBagPlacements()
        {
            var litterBagPlacements = await _litterBagPlacementService.GetLitterBagPlacements();

            return Ok(litterBagPlacements);
        }
    }
}
