using Microsoft.AspNetCore.Mvc;
using Plogging.Core.Models;
using PloggingAPI.Services.Interfaces;

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
    public async Task<ActionResult<IEnumerable<UserRanking>>> Get(int pageNumber = 1, int pageSize = 10)
    {
        var rankings = await _rankingService.GetRankings(pageNumber, pageSize);
        return Ok(rankings);
    }
}
