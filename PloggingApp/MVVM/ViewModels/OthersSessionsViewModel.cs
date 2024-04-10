using Plogging.Core.Models;
using System.Collections.ObjectModel;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Extensions;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.MVVM.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using PloggingApp.Data.Services;
using CommunityToolkit.Maui.Core;

namespace PloggingApp.MVVM.ViewModels;

public partial class OthersSessionsViewModel : BaseViewModel, IAsyncInitialization
{

    public ObservableCollection<PloggingSession> PloggingSessions { get; set; } = [];

    public ObservableCollection<Badge> Badges { get; set; } = [];
    private readonly List<Badge> badges = [];

    [ObservableProperty]
    private double totalSteps;

    [ObservableProperty]
    private double totalDistance;

    [ObservableProperty]
    private double totalCO2Saved;

    [ObservableProperty]
    private double totalWeight;

    [ObservableProperty]
    private string displayName;

    [ObservableProperty]
    private string streakString;

    private IEnumerable<PloggingSession> _allSessions = [];
    public BadgesViewModel BadgesViewModel { get; set; }
    private readonly IPloggingSessionService _sessionService;
    private readonly IStreakService _streakService;
    private readonly IUserInfoService _userInfo;
    private readonly IPopupService _popupService;
    private IRelayCommand? RecentSessionCommand { get; set; }
    public Task Initialization { get; private set; }

    public OthersSessionsViewModel(IPloggingSessionService SessionService, IUserInfoService UserInfo, IStreakService StreakService, IPopupService PopupService)
    {
        _sessionService = SessionService;
        _userInfo = UserInfo;
        _streakService = StreakService;
        _popupService = PopupService;
        BadgesViewModel = new BadgesViewModel(SessionService, UserInfo, StreakService, PopupService);

        Initialization = GetSessions(); //TODO Dela upp i två funktioner?
    }

    [RelayCommand]
    public async Task UpdatePage()
    {
        await GetSessions();
    }

    public async Task GetSessions()
    {
        IsBusy = true;
        string userId = _sessionService.UserId;

        if(userId != null)
        {
            var user = await _userInfo.GetUser(userId);
            DisplayName = user.DisplayName;
            _allSessions = await _sessionService.GetUserSessions(userId, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
            var stats = new PloggingStatistics(_allSessions);
            TotalSteps = Math.Round(stats.TotalSteps);
            TotalDistance = Math.Round(stats.TotalDistance);
            TotalCO2Saved = Math.Round(stats.TotalCO2Saved);
            TotalWeight = Math.Round(stats.TotalWeight);
            
            foreach(PloggingSession ps in _allSessions)
            {
                ps.PloggingData.Distance = Math.Round(ps.PloggingData.Distance);
                ps.PloggingData.Weight = Math.Round(ps.PloggingData.Weight,1);
            }

            PloggingSessions.ClearAndAddRange(_allSessions);
            int streak = (await _streakService.GetUserStreak(userId)).Streak;
            StreakString = streak.ToString();
        }

        IsBusy = false;
    }

}