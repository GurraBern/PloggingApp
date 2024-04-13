using Microsoft.AspNetCore.Mvc;
using Plogging.Core.Models;
using PloggingAPI.Services.Interfaces;

namespace PloggingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserEventsController : Controller
{
    private readonly IUserEventService _userEventService;

    public UserEventsController(IUserEventService userEventService)
    {
        _userEventService = userEventService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserEvent([FromBody] UserEvent userEvent)
    {
        await _userEventService.CreateEvent(userEvent);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserEvent>>> GetUserEvents()
    {
        var userEvents = await _userEventService.GetEvents();

        return Ok(userEvents.ToList());
    }
} 