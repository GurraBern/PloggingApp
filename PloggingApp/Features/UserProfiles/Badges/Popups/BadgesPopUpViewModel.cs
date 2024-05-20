﻿using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Extensions;
using PloggingApp.Features.Statistics;
using PloggingApp.Shared;
using System.Collections.ObjectModel;

namespace PloggingApp.Features.UserProfiles.Badges;

public partial class BadgesPopUpViewModel : BaseViewModel {
    private readonly string _userId;
    private readonly IPloggingSessionService _sessionService;
    private readonly IStreakService _streakService;

    public ObservableCollection<Badge> Badges { get; set; } = [];
    private readonly List<Badge> badges = [];

    public Task Initialization { get; private set; }
    public BadgesPopUpViewModel(string userId, IPloggingSessionService sessionService, IStreakService streakService)
    {
        _userId = userId;
        _sessionService = sessionService;
        _streakService = streakService;

        Initialization = Init();
    }

    public async Task Init()
    {
        IsBusy = true;

        if (_userId != null)
        {
            var _allSessions = await _sessionService.GetUserSessions(_userId, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
            var stats = new PloggingStatistics(_allSessions);
            int streak;
            if (_allSessions.Any())
            {
                stats.TotalSteps = 0;
                stats.TotalDistance = 0;
                stats.TotalCO2Saved = 0;
                stats.TotalWeight = 0;
                stats.TotalTime = TimeSpan.Zero;
                streak = 0;
            }
            else
            {
                streak = (await _streakService.GetUserStreak(_userId)).BiggestStreak;
            }

            await GetBadges(stats, streak);
        }

        IsBusy = false;
    }

    public async Task GetBadges(PloggingStatistics stats, int streak)
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
        if (Badge.Level == Levels.Gold)
        {
            await Application.Current.MainPage.DisplayAlert(Badge.Type, "This user is currently on level " + Badge.Level + " with a total of " + Badge.Progression.ToString() + " " + Badge.Measurement + ", this is the highest level", "OK");
        }
        else if (Badge.Level == Levels.Locked)
        {
            await Application.Current.MainPage.DisplayAlert(Badge.Type, "This user has currently not reached a level and need " + Badge.ToNextLevel.ToString() + " more " + Badge.Measurement + " for the next level", "OK");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(Badge.Type, "This user is currently on level " + Badge.Level + " , the user need " + Badge.ToNextLevel.ToString() + " more " + Badge.Measurement + " for the next level", "OK");
        }
    }

    [RelayCommand]
    public async Task Close(Popup p)
    {
        p.Close();
    }
}