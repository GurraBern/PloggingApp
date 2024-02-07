using Microsoft.AspNetCore.Mvc;
using Plogging.Core.Models;
using PloggingAPI.Services;

namespace PloggingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserRankingController : ControllerBase
{
    private readonly IRankingService _rankingService;

    public UserRankingController(IRankingService rankingService)
    {
        _rankingService = rankingService;
    }

    [HttpGet(Name = "GetRankings")]
    public async Task<ActionResult<IEnumerable<UserRanking>>> Get()
    {
        var rankings = await _rankingService.GetRankings();
        return Ok(rankings);
    }
}
