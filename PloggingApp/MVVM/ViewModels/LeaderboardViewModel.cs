using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Extensions;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class LeaderboardViewModel : IAsyncInitialization
{
    private readonly IRankingService _rankingService;

    public ObservableCollection<UserRanking> Rankings { get; set; } = [];

    public Task Initialization { get; private set; }

    public SortProperty[] SortProperties { get; set; } = (SortProperty[])Enum.GetValues(typeof(SortProperty));
    public SortProperty SelectedSortProperty { get; set; } = SortProperty.ScrapCount;

    public LeaderboardViewModel(IRankingService rankingService)
    {
        _rankingService = rankingService;

        Initialization = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await GetYearlyRankings();
    }

    [RelayCommand]
    private async Task GetMonthlyRankings()
    {
        var rankings = await _rankingService.GetUserRankings(DateTime.UtcNow.FirstDateInMonth(), DateTime.UtcNow.LastDateInMonth(), SelectedSortProperty);
        Rankings.AddRange(rankings);
    }

    [RelayCommand]
    private async Task GetYearlyRankings()
    {
        var rankings = await _rankingService.GetUserRankings(DateTime.UtcNow.FirstDateInYear(), DateTime.UtcNow.LastDateInYear(), SelectedSortProperty);
        Rankings.AddRange(rankings);
    }
}
