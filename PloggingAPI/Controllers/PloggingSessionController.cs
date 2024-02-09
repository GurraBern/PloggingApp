using Microsoft.AspNetCore.Mvc;
using Plogging.Core.Models;
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PloggingSession>>> Get()
    {
        var sessions = await _ploggingSessionService.GetSessionSummaries();
        return Ok(sessions);
    }
}
