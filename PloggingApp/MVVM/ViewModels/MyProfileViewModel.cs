using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Services.Authentication;
using PloggingApp.Extensions;
using Plogging.Core.Models;
using PloggingApp.Pages;
using System.Collections.ObjectModel;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.Interfaces;
using System.Diagnostics;
using PloggingApp.MVVM.Models;

namespace PloggingApp.MVVM.ViewModels;

public partial class MyProfileViewModel : BaseViewModel, IAsyncInitialization
{

    private readonly IPloggingSessionService _ploggingSessionService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IRankingService _rankingService;

    public PloggingSessionViewModel PloggingSessionViewModel { get; }
    public StreakViewModel StreakViewModel { get; set; }
    public LeaderboardViewModel LeaderboardViewModel { get; }

    public ObservableCollection<PloggingSession> PloggingSessions { get; set; } = [];
    public IEnumerable<PloggingSession> _allUserSessions = new ObservableCollection<PloggingSession>();

    [ObservableProperty]
    public string displayName;
    [ObservableProperty]
    public double totalSteps;
    [ObservableProperty]
    public double totalDistance;
    [ObservableProperty]
    public double totalCO2Saved;
    [ObservableProperty]
    public double totalWeight;
    [ObservableProperty]
    public double totalTime;
    [ObservableProperty]
    public int userRankInt;
    [ObservableProperty]
    public DateTime recentStartDate0;
    [ObservableProperty]
    public DateTime recentStartDate1;
    [ObservableProperty]
    public DateTime recentStartDate2;
    [ObservableProperty]
    public double recentWeight0;
    [ObservableProperty]
    public double recentWeight1;
    [ObservableProperty]
    public double recentWeight2;

    public MyProfileViewModel(IAuthenticationService authenticationService, IRankingService rankingService, StreakViewModel streakViewModel, IPloggingSessionService ploggingSessionService, PloggingSessionViewModel ploggingSessionViewModel, LeaderboardViewModel leaderboardViewModel)
    {
        _authenticationService = authenticationService;
        _rankingService = rankingService;
        _ploggingSessionService = ploggingSessionService;
        StreakViewModel = streakViewModel;
        PloggingSessionViewModel = ploggingSessionViewModel;
        LeaderboardViewModel = leaderboardViewModel;

        Initialization = InitializeAsync();
    }

    public Task Initialization { get; private set; }

    private async Task InitializeAsync()
    {
        await GetSessions();
    }
    public async Task GetSessions()
    {
        IsBusy = true;
        _allUserSessions = await _ploggingSessionService.GetUserSessions(_authenticationService.CurrentUser.Uid, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
        
        RecentStartDate0 = _allUserSessions.ElementAt(0).StartDate;
        RecentStartDate1 = _allUserSessions.ElementAt(1).StartDate;
        RecentStartDate2 = _allUserSessions.ElementAt(2).StartDate;

        RecentWeight0 = Math.Round(_allUserSessions.ElementAt(0).PloggingData.Weight);
        RecentWeight1 = Math.Round(_allUserSessions.ElementAt(1).PloggingData.Weight);
        RecentWeight2 = Math.Round(_allUserSessions.ElementAt(2).PloggingData.Weight);

        DisplayName = _authenticationService.CurrentUser.Info.DisplayName;
        PloggingSessions.ClearAndAddRange(_allUserSessions);

        UserRankInt = LeaderboardViewModel.UserRank.Rank;

        var stats = new PloggingStatistics(_allUserSessions);
        TotalDistance = Math.Round(stats.TotalDistance);
        TotalCO2Saved = Math.Round(stats.TotalCO2Saved);
        TotalWeight = Math.Round(stats.TotalWeight);
        

        IsBusy = false;
    }

    [RelayCommand]
    private async Task Logout()
    {
        _authenticationService.SignOut();
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }

    [RelayCommand]
    public async Task GoToMyProfilePage()
    {
        await Shell.Current.GoToAsync("MyProfilePage");
    }



}

