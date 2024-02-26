using Microsoft.AspNetCore.Mvc;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingAPI.Models.Queries;
using PloggingAPI.Services.Interfaces;

namespace PloggingAPI.Controllers;

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
    public async Task<ActionResult<IEnumerable<PloggingSession>>> GetSessionSummaries(DateTime startDate, DateTime endDate, SortDirection sortDirection = SortDirection.Descending, SortProperty sortProperty = SortProperty.ScrapCount)
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

    [HttpGet("UserSessions/{userId}")]
    public async Task<ActionResult<IEnumerable<PloggingSession>>> GetUserSessions(string userId, DateTime startDate, DateTime endDate)
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
    public async Task<ActionResult> CreatePloggingSession([FromBody] PloggingSession ploggingSession)
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
}
