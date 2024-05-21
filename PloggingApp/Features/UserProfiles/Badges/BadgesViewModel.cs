using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Features.Statistics;
using PloggingApp.Services.Authentication;
using PloggingApp.Shared;
using System.Collections.ObjectModel;

namespace PloggingApp.Features.UserProfiles.Badges;

public partial class BadgesViewModel: BaseViewModel
{
    public ObservableCollection<Badge> Badges { get; set; } = [];
    private readonly IPloggingSessionService _sessionService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IStreakService _streakService;
    private readonly IPopupService _popupService;
    public Task Initialization { get; private set; }

    private string UserId => _authenticationService.UserId;

    public BadgesViewModel(IPloggingSessionService sessionService, IAuthenticationService authenticationService, IStreakService streakService, IPopupService popupService)
    {
        _sessionService = sessionService;
        _authenticationService = authenticationService;
        _streakService = streakService;
        _popupService = popupService;

        Initialization = Init();
    }

    public async Task Init()
    {
        IsBusy = true;

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
