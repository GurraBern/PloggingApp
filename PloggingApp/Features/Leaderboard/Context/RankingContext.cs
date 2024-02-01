using PloggingApp.Data.Context.Interfaces;
using PloggingApp.Shared.Models;

namespace PloggingApp.Data.Context;

public class RankingContext : IRankingContext
{
    //TODO Change to fetch rankings from database instead
    public IEnumerable<Ranking> Rankings
    {
        get
        {
            return new List<Ranking>()
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

    //TODO should communicate to a service
    public RankingContext()
    {

    }
}