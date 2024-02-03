using Microsoft.AspNetCore.Mvc;
using Plogging.Core.Models;

namespace PloggingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserRankingController : ControllerBase
{
    public UserRankingController()
    {
    }

    [HttpGet(Name = "GetRankings")]
    public IEnumerable<UserRanking> Get()
    {
        return new List<UserRanking>()
        {
            new()
            {
                DisplayName = "Test Name",
                ScrapCount = 123,
                Steps = 25004,
                Distance = 3574
            },
            new()
            {
                DisplayName = "Test Name2",
                ScrapCount = 157,
                Steps = 35683,
                Distance = 6574
            },
            new()
            {
                DisplayName = "Test Name3",
                ScrapCount = 26,
                Steps = 1549,
                Distance = 734
            }
        };
    }
}
