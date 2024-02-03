using Plogging.Core.Models;
using PloggingApp.Data.Services;

namespace PloggingApp.MVVM.ViewModels;

public class LeaderboardViewModel : IAsyncInitialization
{
    private readonly IRankingService _rankingService;

    public IEnumerable<UserRanking> Rankings { get; }

    public Task Initialization { get; private set; }

    public LeaderboardViewModel(IRankingService rankingService)
    {
        _rankingService = rankingService;

        Initialization = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await _rankingService.GetUserRankings();
    }
}
