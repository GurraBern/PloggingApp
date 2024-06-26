﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Services.Authentication;
using PloggingApp.Extensions;
using Plogging.Core.Models;
using System.Collections.ObjectModel;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.Models;
using PloggingApp.Pages;
using PloggingApp.Services;

namespace PloggingApp.MVVM.ViewModels;

public partial class MyProfileViewModel : BaseViewModel, IAsyncInitialization
{
    private readonly IPloggingSessionService _ploggingSessionService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IRankingService _rankingService;
    private readonly IStreakService _streakService;
    private readonly IToastService _toastService;

    public PloggingSessionViewModel PloggingSessionViewModel { get; }
    public StreakViewModel StreakViewModel { get; set; }
    public LeaderboardViewModel LeaderboardViewModel { get; }
    public BadgesViewModel BadgesViewModel { get; }

    public ObservableCollection<PloggingSession> PloggingSessions { get; set; } = [];

    public IEnumerable<PloggingSession> _allUserSessions = new ObservableCollection<PloggingSession>();

    [ObservableProperty]
    PloggingStatistics ploggingStatistics;
    [ObservableProperty]
    public string displayName;
    [ObservableProperty]
    public double totalDistance;
    [ObservableProperty]
    public double totalCO2Saved;
    [ObservableProperty]
    public double totalWeight;
    [ObservableProperty]
    public int userRankInt;
    [ObservableProperty]
    private bool isRefreshing;
    [ObservableProperty]
    public IEnumerable<PloggingSession> latestSessions;


    public MyProfileViewModel(IAuthenticationService authenticationService, 
        IRankingService rankingService,
        IStreakService StreakService,
        StreakViewModel streakViewModel, 
        IPloggingSessionService ploggingSessionService, 
        PloggingSessionViewModel ploggingSessionViewModel, 
        LeaderboardViewModel leaderboardViewModel,
        BadgesViewModel badgesViewModel,
        IToastService toastService)
    {
        _authenticationService = authenticationService;
        _rankingService = rankingService;
        _ploggingSessionService = ploggingSessionService;
        StreakViewModel = streakViewModel;
        PloggingSessionViewModel = ploggingSessionViewModel;
        LeaderboardViewModel = leaderboardViewModel;
        BadgesViewModel = badgesViewModel;
        _streakService = StreakService;
        _toastService = toastService;

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

        DisplayName = _authenticationService.CurrentUser.Info.DisplayName;
        _ploggingSessionService.MyUserId = _authenticationService.CurrentUser.Uid;
        _ploggingSessionService.UserId = _ploggingSessionService.MyUserId;

        await BadgesViewModel.Init();

        _allUserSessions = await _ploggingSessionService.GetUserSessions(_authenticationService.CurrentUser.Uid, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);

        if (!_allUserSessions.Any())
        {
            IsBusy = false;
            return;
        }
        else
        {
            PloggingSessions.ClearAndAddRange(_allUserSessions);
            PloggingStatistics = new PloggingStatistics(_allUserSessions.Where(s =>s.StartDate.Month == DateTime.Now.Month));
            LatestSessions = PloggingSessions.Take(3);
            UserRankInt = LeaderboardViewModel.UserRank.Rank;

            IsBusy = false;
        }
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
    private async Task Refresh()
    {
        IsBusy = true;
        await GetSessions();
        IsRefreshing = false;
        IsBusy = false;
    }

    [RelayCommand]
    private async Task GoToHistoryPage()
    {
        await Shell.Current.GoToAsync($"{nameof(HistoryPage)}");
    }
}

