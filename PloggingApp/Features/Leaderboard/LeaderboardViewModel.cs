using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Extensions;
using System.Collections.ObjectModel;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Pages;
using PloggingApp.MVVM.ViewModels;
using PloggingApp.Shared;
using PloggingApp.Services;

namespace PloggingApp.Features.Leaderboard;

public partial class LeaderboardViewModel : BaseViewModel, IAsyncInitialization
{
    private readonly IRankingService _rankingService;
    private readonly IPloggingSessionService _sessionService;
    private readonly IUserInfoService _userInfo;
    private readonly IToastService _toastService;

    public ObservableCollection<UserRanking> Rankings { get; set; } = [];
    public SortProperty[] SortProperties { get; set; } = (SortProperty[])Enum.GetValues(typeof(SortProperty));
    public SortProperty SelectedSortProperty
    {
        get => _selectedSortProperty;
        set
        {
            _selectedSortProperty = value;
            SortUnit = value.GetUnitOfMeasurement();

            if (RecentRankingCommand != null && RecentRankingCommand.CanExecute(this))
            {
                RecentRankingCommand.Execute(this);
            }

            OnPropertyChanged(nameof(SortUnit));
            OnPropertyChanged(nameof(SelectedSortProperty));
        }
    }
    private SortProperty _selectedSortProperty;
    public string SortUnit { get; set; } = "";
    private IRelayCommand? RecentRankingCommand { get; set; }
    public Task Initialization { get; private set; }

    [ObservableProperty]
    private UserRanking userRank;

    public LeaderboardViewModel(IRankingService rankingService, IPloggingSessionService sessionService, IUserInfoService userInfo, IToastService toastService)
    {
        _rankingService = rankingService;
        _sessionService = sessionService;
        _userInfo = userInfo;
        _toastService = toastService;

        Initialization = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await GetYearlyRankings();
    }

    [RelayCommand]
    private async Task GetMonthlyRankings()
    {
        IsBusy = true;

        await GetRankings(DateTime.UtcNow.FirstDateInMonth(), DateTime.UtcNow.LastDateInMonth(), SelectedSortProperty, GetMonthlyRankingsCommand);

        IsBusy = false;
    }

    [RelayCommand]
    private async Task GetYearlyRankings()
    {
        IsBusy = true;

        await GetRankings(DateTime.UtcNow.FirstDateInYear(), DateTime.UtcNow.LastDateInYear(), SelectedSortProperty, GetYearlyRankingsCommand);

        IsBusy = false;
    }

    [RelayCommand]
    private async Task GetAllTimeRankings()
    {
        IsBusy = true;

        await GetRankings(DateTime.MinValue, DateTime.UtcNow.LastDateInYear(), SelectedSortProperty, GetAllTimeRankingsCommand);

        IsBusy = false;
    }

    private async Task GetRankings(DateTime startDate, DateTime endDate, SortProperty sortProperty, IAsyncRelayCommand command)
    {
        RecentRankingCommand = command;

        var userRankings = await _rankingService.GetUserRankings(startDate, endDate, sortProperty);
        if (!userRankings.Any())
        {
            await _toastService.MakeToast("Could not fetch user rankings");
        }

        Rankings.ClearAndAddRange(userRankings);
        UserRank = _rankingService.UserRank;
    }

    [RelayCommand]
    public void SearchUsers(string userName)
    {
        if (!userName.Equals(""))
        {
            var searchResults = _rankingService.UserRankings.Where(x => x.DisplayName != null && x.DisplayName.Contains(userName, StringComparison.InvariantCultureIgnoreCase));
            Rankings.ClearAndAddRange(searchResults);
        }
        else
        {
            Rankings.ClearAndAddRange(_rankingService.UserRankings);
        }
    }

    [RelayCommand]
    private async Task GoToProfilePage(string userId)
    {
        IsBusy = true;
        var user = await _userInfo.GetUser(userId);
        if (user == null)
        {
            IsBusy = false;
            await Application.Current.MainPage.DisplayAlert("ERROR", "Can not show profile, user does not exist.", "OK");
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(OthersProfilePage)}?UserId={userId}");
        IsBusy = false;
    }
}
