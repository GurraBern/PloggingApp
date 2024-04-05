using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Services.Authentication;
using PloggingApp.Extensions;
using Plogging.Core.Models;
using PloggingApp.Pages;
using System.Collections.ObjectModel;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.Models;

namespace PloggingApp.MVVM.ViewModels;

public partial class MyProfileViewModel : BaseViewModel, IAsyncInitialization
{
    private readonly IPloggingSessionService _ploggingSessionService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IRankingService _rankingService;
    private readonly IStreakService _streakService;

    public PloggingSessionViewModel PloggingSessionViewModel { get; }
    public StreakViewModel StreakViewModel { get; set; }
    public LeaderboardViewModel LeaderboardViewModel { get; }
    public BadgesViewModel BadgesViewModel { get; }

    public ObservableCollection<PloggingSession> PloggingSessions { get; set; } = [];
    public IEnumerable<PloggingSession> _allUserSessions = new ObservableCollection<PloggingSession>();


    [ObservableProperty]
    public string displayName;
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
    [ObservableProperty]
    private bool isRefreshing;


    public ObservableCollection<Badge> Badges { get; set; } = [];
    public List<Badge> _Badges { get; set; } = [];

    public MyProfileViewModel(IAuthenticationService authenticationService, 
        IRankingService rankingService,
        IStreakService StreakService,
        StreakViewModel streakViewModel, 
        IPloggingSessionService ploggingSessionService, 
        PloggingSessionViewModel ploggingSessionViewModel, 
        LeaderboardViewModel leaderboardViewModel,
        BadgesViewModel badgesViewModel)
    {
        _authenticationService = authenticationService;
        _rankingService = rankingService;
        _ploggingSessionService = ploggingSessionService;
        StreakViewModel = streakViewModel;
        PloggingSessionViewModel = ploggingSessionViewModel;
        LeaderboardViewModel = leaderboardViewModel;
        BadgesViewModel = badgesViewModel;
        _streakService = StreakService;

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

        int streak = (await _streakService.GetUserStreak(_authenticationService.CurrentUser.Uid)).Streak;

        //BADGES
        await BadgesViewModel.GetBadges(stats, streak);

        IsBusy = false;
    }

    [RelayCommand]
    private async Task Logout()
    {
        bool response = await Application.Current.MainPage.DisplayAlert("Signing out", "Are you sure you want to logout?", "Yes", "No");

        if (response)
        {
            _authenticationService.SignOut();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }

    [RelayCommand]
    public async Task GoToMyProfilePage()
    {
        await Shell.Current.GoToAsync("MyProfilePage");
    }

    [RelayCommand]
    private async Task RefreshMyProfile()
    {
        IsRefreshing = true;
        await GetSessions();
        IsRefreshing = false;
    }

}

