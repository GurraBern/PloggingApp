using Plogging.Core.Models;
using System.Collections.ObjectModel;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Extensions;
using CommunityToolkit.Mvvm.Input;

using Plogging.Core.Enums;
using PloggingApp.MVVM.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using PloggingApp.Data.Services;
using Firebase.Auth.Requests;
using Firebase.Auth;
using System.Runtime.CompilerServices;
using PloggingApp.Pages;
using CommunityToolkit.Maui.Core;
using Syncfusion.Maui.Core.Carousel;

namespace PloggingApp.MVVM.ViewModels;

public partial class OthersSessionsViewModel : BaseViewModel, IAsyncInitialization
{

    public ObservableCollection<PloggingSession> PloggingSessions { get; set; } = [];

    public ObservableCollection<Badge> Badges { get; set; } = [];
    public List<Badge> _Badges { get; set; } = [];

    [ObservableProperty]
    public double totalSteps;
    [ObservableProperty]
    public double totalDistance;
    [ObservableProperty]
    public double totalCO2Saved;
    [ObservableProperty]
    public double totalWeight;
    [ObservableProperty]
    public string displayName;

    [ObservableProperty]
    public string streakString;


    private IEnumerable<PloggingSession> _allSessions = new ObservableCollection<PloggingSession>();

    private readonly IPloggingSessionService _sessionService;
    private readonly IStreakService _streakService;
    private readonly IUserInfoService _userInfo;
    private readonly IPopupService _popupService;
    public Plogging.Core.Models.UserInfo user;
    string userPassed;
    private IRelayCommand? RecentSessionCommand { get; set; }
    public Task Initialization { get; private set; }

    public OthersSessionsViewModel(IPloggingSessionService SessionService, IUserInfoService UserInfo, IStreakService StreakService, IPopupService PopupService, string userPassed_)
    {
        _sessionService = SessionService;
        _userInfo = UserInfo;
        _streakService = StreakService;
        _popupService = PopupService;
        this.userPassed = userPassed_;
        Initialization =GetSessionsAndBadges(); //Dela upp i två funktioner?
    }

    [RelayCommand]
    public async Task UpdatePage()
    {

        await GetSessionsAndBadges();

    }

    public async Task GetSessionsAndBadges()
    {
        IsBusy = true;
        //string userId = _sessionService.UserId;
        user = await _userInfo.GetUser(userPassed);
        DisplayName = user.DisplayName;
        _allSessions = await _sessionService.GetUserSessions(userPassed, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
        var stats = new PloggingStatistics(_allSessions);
        TotalSteps = Math.Round(stats.TotalSteps);
        TotalDistance = Math.Round(stats.TotalDistance);
        TotalCO2Saved = Math.Round(stats.TotalCO2Saved);
        TotalWeight = Math.Round(stats.TotalWeight);
        PloggingSessions.ClearAndAddRange(_allSessions);
        int streak = (await _streakService.GetUserStreak(userPassed)).BiggestStreak;
        StreakString = streak.ToString();
        await GetBadges(userPassed, _allSessions, stats, streak);
        IsBusy = false;
    }

    public async Task GetBadges(string userPassed, IEnumerable<PloggingSession> _allSessions, PloggingStatistics stats, int streak)
    {
        _Badges.Add(new TrashCollectedBadge(stats));
        _Badges.Add(new DistanceBadge(stats));
        _Badges.Add(new TimeSpentBadge(stats));
        _Badges.Add(new CO2Badge(stats));
        _Badges.Add(new StreakBadge(streak));
        Badges.ClearAndAddRange(_Badges);
        _Badges.Clear();
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