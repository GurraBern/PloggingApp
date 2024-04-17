using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Extensions;
using PloggingApp.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PloggingApp.MVVM.ViewModels;

public partial class BadgesViewModel: BaseViewModel
{
    public ObservableCollection<Badge> Badges { get; set; } = [];
    private readonly List<Badge> badges = [];
    private readonly IPloggingSessionService _sessionService;
    private readonly IStreakService _streakService;
    private readonly IUserInfoService _userInfo;
    private readonly IPopupService _popupService;
    public Task Initialization { get; private set; }
    public BadgesViewModel(IPloggingSessionService SessionService, IUserInfoService UserInfo, IStreakService StreakService, IPopupService PopupService)
    {
        _sessionService = SessionService;
        _userInfo = UserInfo;
        _streakService = StreakService;
        _popupService = PopupService;

        Initialization = Init(); 
    }

    public async Task Init()
    {
        IsBusy = true;
        string userId = _sessionService.UserId;

        if (userId != null)
        {
            var _allSessions = await _sessionService.GetUserSessions(userId, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
            var stats = new PloggingStatistics(_allSessions);
            int streak;
            if (_allSessions.Count() == 0) 
            {
                stats.totalSteps = 0;
                stats.totalDistance = 0;
                stats.totalCO2Saved = 0;
                stats.totalWeight = 0;
                stats.totalTime = TimeSpan.Zero;
                streak = 0;
            }
            else
            {
            streak = (await _streakService.GetUserStreak(userId)).BiggestStreak;
            }
            
            await GetBadges(stats, streak);
        }

        IsBusy = false;
    }

    public async Task GetBadges(PloggingStatistics stats, int streak)
    {
        Badges.Clear();
        badges.Add(new TrashCollectedBadge(stats));
        badges.Add(new DistanceBadge(stats));
        badges.Add(new TimeSpentBadge(stats));
        badges.Add(new CO2Badge(stats));
        badges.Add(new StreakBadge(streak));
        foreach (Badge b in badges)
        {
                Badges.Add(b);

            if (Badges.Count == 5)
            {
                return;
            }
        }

        badges.Clear();
    }

    [RelayCommand]
    public async Task TapBadge(Badge Badge)
    {
        if (Badge.Level == Levels.Gold)
        {
            await Application.Current.MainPage.DisplayAlert(Badge.Type, "This user is currently on level " + Badge.Level + " with a total of " + Badge.progression.ToString() + " " + Badge.Measurement + ", this is the highest level", "OK");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Badge.Type, "This user is currently on level " + Badge.Level + " , the user need " + Badge.ToNextLevel.ToString() + " more " + Badge.Measurement + " for the next level", "OK");
        }
    }

    [RelayCommand]
    public async Task ShowBadges()
    {
        await _popupService.ShowPopupAsync<BadgesPopUpViewModel>();
    }



}
