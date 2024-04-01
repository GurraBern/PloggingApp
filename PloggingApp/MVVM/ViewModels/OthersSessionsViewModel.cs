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

        Initialization = GetSessionsAndBadges(); //TODO Dela upp i två funktioner?
    }

    [RelayCommand]
    public async Task UpdatePage()
    {
        await GetSessionsAndBadges();
    }

    public async Task GetSessionsAndBadges()
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
            PloggingSessions.ClearAndAddRange(_allSessions);
            int streak = (await _streakService.GetUserStreak(userId)).Streak;
            StreakString = streak.ToString();
            await GetBadges(userId, _allSessions, stats, streak);
        }

        IsBusy = false;
    }

    public async Task GetBadges(string UserId, IEnumerable<PloggingSession> _allSessions, PloggingStatistics stats, int streak)
    {
        badges.Add(new TrashCollectedBadge(stats));
        badges.Add(new DistanceBadge(stats));
        badges.Add(new TimeSpentBadge(stats));
        badges.Add(new CO2Badge(stats));
        badges.Add(new StreakBadge(streak));
        Badges.ClearAndAddRange(badges);
        badges.Clear();
    }

    [RelayCommand]
    public async Task TapBadge(Badge Badge)
    {
        if (Badge.Level == "Gold")
        {
            await Application.Current.MainPage.DisplayAlert(Badge.Type, "This user is currently on level " + Badge.Level + " with a total of " + Badge.progression.ToString() + " " + Badge.Measurement  + ", this is the highest level", "OK");
        }
        else if (Badge.Level == "null")
        {
            await Application.Current.MainPage.DisplayAlert(Badge.Type, "This user has currently not reached a level and need " + Badge.ToNextLevel.ToString() +" more " + Badge.Measurement + " for the next level", "OK");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Badge.Type, "This user is currently on level " + Badge.Level + " , the user need " + Badge.ToNextLevel.ToString() +" more " + Badge.Measurement + " for the next level", "OK");
        }
    }

    [RelayCommand]
    public async Task ShowBadges()
    {
        await _popupService.ShowPopupAsync<BadgesPopUpViewModel>(onPresenting: viewModel => viewModel.Badges = Badges);
    }
}