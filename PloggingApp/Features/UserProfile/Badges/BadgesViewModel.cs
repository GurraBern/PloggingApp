using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.Models;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class BadgesViewModel: BaseViewModel
{
    public ObservableCollection<Badge> Badges { get; set; } = [];
    //private readonly List<Badge> badges = [];
    private readonly IPloggingSessionService _sessionService;
    private readonly IStreakService _streakService;
    private readonly IUserInfoService _userInfo;
    private readonly IPopupService _popupService;
    public Task Initialization { get; private set; }

    [ObservableProperty]
    private string userId;

    public BadgesViewModel(IPloggingSessionService sessionService, IUserInfoService userInfo, IStreakService streakService, IPopupService popupService)
    {
        _sessionService = sessionService;
        _userInfo = userInfo;
        _streakService = streakService;
        _popupService = popupService;

        Initialization = Init(); 
    }

    public async Task Init()
    {
        IsBusy = true;
        //string userId = _sessionService.UserId;

        if (UserId != null)
        {
            var _allSessions = await _sessionService.GetUserSessions(UserId, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
            var ploggingStatistics = new PloggingStatistics(_allSessions);
            int streak;
            if (!_allSessions.Any())
            {
                ploggingStatistics.TotalSteps = 0;
                ploggingStatistics.TotalDistance = 0;
                ploggingStatistics.TotalCO2Saved = 0;
                ploggingStatistics.TotalWeight = 0;
                ploggingStatistics.TotalTime = TimeSpan.Zero;
                streak = 0;
            }
            else
            {
                streak = (await _streakService.GetUserStreak(UserId)).BiggestStreak;
            }
            
            GetBadges(ploggingStatistics, streak);
        }

        IsBusy = false;
    }

    public void GetBadges(PloggingStatistics stats, int streak)
    {
        Badges.Clear();
        Badges.Add(new TrashCollectedBadge(stats));
        Badges.Add(new DistanceBadge(stats));
        Badges.Add(new TimeSpentBadge(stats));
        Badges.Add(new CO2Badge(stats));
        Badges.Add(new StreakBadge(streak));

        foreach (Badge badge in Badges)
        {
            Badges.Add(badge);

            if (Badges.Count == 5)
            {
                return;
            }
        }

        Badges.Clear();
    }

    [RelayCommand]
    public static async Task TapBadge(Badge badge)
    {
        if (badge.Level == Levels.Gold)
        {
            await Application.Current.MainPage.DisplayAlert(badge.Type, "This user is currently on level " + badge.Level + " with a total of " + badge.Progression.ToString() + " " + badge.Measurement + ", this is the highest level", "OK");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(badge.Type, "This user is currently on level " + badge.Level + " , the user need " + badge.ToNextLevel.ToString() + " more " + badge.Measurement + " for the next level", "OK");
        }
    }

    [RelayCommand]
    public async Task ShowBadges()
    {
        await _popupService.ShowPopupAsync<BadgesPopUpViewModel>();
    }
}
