using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Extensions;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;

public class LeaderboardViewModel : IAsyncInitialization
{
    private readonly IRankingService _rankingService;

    public ObservableCollection<UserRanking> Rankings { get; set; } = [];

    public Task Initialization { get; private set; }

    public LeaderboardViewModel(IRankingService rankingService)
    {
        _rankingService = rankingService;

        Initialization = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        var rankings = await _rankingService.GetUserRankings();
        Rankings.AddRange(rankings);
    }
}
