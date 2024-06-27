using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlogPal.Common.Enums;
using PlogPal.Domain.Models;

namespace PloggingAPI.Features.PloggingSession;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PloggingSessionController : ControllerBase
{
    private readonly IPloggingSessionService _ploggingSessionService;

    public PloggingSessionController(IPloggingSessionService ploggingSessionService)
    {
        _ploggingSessionService = ploggingSessionService;
    }

    [HttpGet("Summary")]
    public async Task<ActionResult<IEnumerable<PlogSession>>> GetSessionSummaries(DateTime startDate, DateTime endDate, SortDirection sortDirection = SortDirection.Descending, SortProperty sortProperty = SortProperty.Weight)
    {
        var query = new SessionSummaryQuery()
        {
            SortDirection = sortDirection,
            SortProperty = sortProperty,
            StartDate = startDate,
            EndDate = endDate
        };

        try
        {
            var sessions = await _ploggingSessionService.GetSessionSummaries(query);
            return Ok(sessions);
        }
        catch (Exception ex)
        {
            //TODO add logging
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("UserSessions")]
    public async Task<ActionResult<IEnumerable<PlogSession>>> GetUserSessions(string userId, DateTime startDate, DateTime endDate)
    {

        try
        {
            var sessions = await _ploggingSessionService.GetPloggingSessions(userId, startDate, endDate);

            return Ok(sessions);
        }
        catch (Exception ex)
        {
            //TODO add logging
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost("UserSessions")]
    public async Task<IActionResult> CreatePloggingSession([FromBody] PlogSession ploggingSession)
    {
        try
        {
            await _ploggingSessionService.AddPloggingSession(ploggingSession);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("LitterLocations")]
    public async Task<ActionResult<IEnumerable<LitterLocation>>> GetLitterLocations()
    {
        var litterLocations = await _ploggingSessionService.GetLitterLocations();
        return Ok(litterLocations);
    }
}
