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
        UserSessions.ClearAndAddRange(_allUserSessions);
        chartService = new ChartService(UserSessions);
        PloggingStats = new PloggingStatistics(UserSessions);
        TimeRes = TimeResolution.ThisYear;
        DistanceChart = new ChartContext
        {
            Chart = chartService.generateDistanceChart(TimeRes),
            Name = "Distance",
            Unit = "km"
        };
        LitterChart = new ChartContext
        {
            Chart = chartService.generateLitterChart(TimeRes),
            Name = "Litter",
            Unit = "pcs"
        };
        IsBusy = false;
    }
    [RelayCommand]
    private void GetMonthChart()
    {
        IsBusy = true;
        TimeRes = TimeResolution.ThisMonth;
        DistanceChart.Chart = chartService.generateDistanceChart(TimeResolution.ThisMonth);
        LitterChart.Chart = chartService.generateLitterChart(TimeResolution.ThisMonth);
        PloggingStats.changeTimeResolution(TimeRes);
        StatsBoxColor = colorDict[TimeRes];
        IsBusy = false;
    }

    [RelayCommand]
    private void GetYearChart()
    {
        IsBusy= true;
        TimeRes = TimeResolution.ThisYear;
        DistanceChart.Chart = chartService.generateDistanceChart(TimeResolution.ThisYear);
        LitterChart.Chart = chartService.generateLitterChart(TimeResolution.ThisYear);
        PloggingStats.changeTimeResolution(TimeRes);
        StatsBoxColor = colorDict[TimeResolution.ThisYear];
        IsBusy= false;
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
