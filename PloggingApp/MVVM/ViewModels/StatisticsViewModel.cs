﻿using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using SkiaSharp;
using Plogging.Core.Models;
using Plogging.Core.Enums;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Extensions;
using System.Collections.ObjectModel;
using PloggingApp.Services.Statistics;
using CommunityToolkit.Mvvm.Input;
using PloggingApp.MVVM.Models;
using PloggingApp.Services.Authentication;
using PloggingApp.Pages;

namespace PloggingApp.MVVM.ViewModels;

public partial class StatisticsViewModel : BaseViewModel, IAsyncInitialization
{

    public Task Initialization { get; private set; }

    private readonly IPloggingSessionService _ploggingSessionService;
    private IChartService chartService;
    private readonly IAuthenticationService _authenticationService;
    public ObservableCollection<PloggingSession> UserSessions { get; set; } = [];
    private IEnumerable<PloggingSession> _allUserSessions = new ObservableCollection<PloggingSession>();

    private Dictionary<TimeResolution, string> colorDict = new Dictionary<TimeResolution, string>
    {
        {TimeResolution.ThisYear,"#5c5aa8" },
        {TimeResolution.ThisMonth, "#9558a8"}
    };
 
    public StatisticsViewModel(IPloggingSessionService ploggingSessionService, IAuthenticationService authenticationService)
    {
        _ploggingSessionService = ploggingSessionService;
        _authenticationService = authenticationService;
        Initialization = InitializeAsync();
    }
    private async Task InitializeAsync()
    {
        await GetUserSessions();
    }
    private async Task GetUserSessions()
    {
        IsBusy = true;
        _allUserSessions = await _ploggingSessionService.GetUserSessions(_authenticationService.CurrentUser.Uid, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
        //_allUserSessions = await _ploggingSessionService.GetUserSessions("newId", DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
        UserSessions.ClearAndAddRange(_allUserSessions);
        chartService = new ChartService();
        PloggingStats = new PloggingStatistics(UserSessions);
        TimeRes = TimeResolution.ThisYear;
        StatsBoxColor = colorDict[TimeRes];
        DistanceChart = new ChartContext
        {
            Chart = chartService.generateDistanceChart(TimeRes,UserSessions),
            Name = "Distance",
            Unit = "km"
        };
        LitterChart = new ChartContext
        {
            Chart = chartService.generateLitterChart(TimeRes, UserSessions),
            Name = "Litter",
            Unit = "pcs"
        };
        IsBusy = false;
    }
    [RelayCommand]
    private void GetMonthChart()
    {
        Update(TimeResolution.ThisMonth);
    }

    [RelayCommand]
    private void GetYearChart()
    {
        Update(TimeResolution.ThisYear);
    }

    private void Update(TimeResolution tr)
    {
        IsBusy = true;
        DistanceChart.Chart = chartService.generateDistanceChart(tr, UserSessions);
        LitterChart.Chart = chartService.generateLitterChart(tr, UserSessions);
        PloggingStats.changeTimeResolution(tr);
        StatsBoxColor = colorDict[tr];
        IsBusy = false;
    }
    [RelayCommand]
    private async Task GoToSessionStats(PloggingSession session)
    {
        if (session is null)
            return;
        await Shell.Current.GoToAsync($"{nameof(SessionStatisticsPage)}", true, 
            new Dictionary<string, object>
            {
                {nameof(PloggingSession), session}
            });
    }

    [ObservableProperty]
    ChartContext distanceChart;
    [ObservableProperty]
    ChartContext litterChart;

    [ObservableProperty]
    TimeResolution timeRes;

    [ObservableProperty]
    ObservableCollection<ChartContext> charts;

    [ObservableProperty]
    PloggingStatistics ploggingStats;

    [ObservableProperty]
    string statsBoxColor;
}
