using CommunityToolkit.Mvvm.ComponentModel;
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

namespace PloggingApp.MVVM.ViewModels;
public partial class StatisticsViewModel : BaseViewModel, IAsyncInitialization
{
    public Task Initialization { get; private set; }

    private readonly IPloggingSessionService _ploggingSessionService;
    private IChartService chartService;
    public ObservableCollection<PloggingSession> UserSessions { get; set; } = [];
    private IEnumerable<PloggingSession> _allUserSessions = new ObservableCollection<PloggingSession>();
    
 
    public StatisticsViewModel(IPloggingSessionService ploggingSessionService)
    {
        _ploggingSessionService = ploggingSessionService;
        Initialization = InitializeAsync();
    }
    private async Task InitializeAsync()
    {
        await GetUserSessions();
    }
    private async Task GetUserSessions()
    {
        IsBusy = true;
        _allUserSessions = await _ploggingSessionService.GetUserSessions("TODOsetUserId", DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
        UserSessions.ClearAndAddRange(_allUserSessions);
        chartService = new ChartService(UserSessions);

        PloggingStats = new PloggingStatistics(UserSessions);
        TimeRes = TimeResolution.ThisYear;

        TotalDistance = PloggingStats.Distance.year;
        TotalCO2Saved = PloggingStats.CO2Saved.year;
        TotalSteps = PloggingStats.Steps.year;
        TotalWeight = PloggingStats.Weight.year;
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

        TotalDistance = PloggingStats.Distance.month;
        TotalCO2Saved = PloggingStats.CO2Saved.month;
        TotalSteps = PloggingStats.Steps.month;
        TotalWeight = PloggingStats.Weight.month;

        IsBusy = false;
    }

    [RelayCommand]
    private void GetYearChart()
    {
        IsBusy= true;
        TimeRes = TimeResolution.ThisYear;
        DistanceChart.Chart = chartService.generateDistanceChart(TimeResolution.ThisYear);
        LitterChart.Chart = chartService.generateLitterChart(TimeResolution.ThisYear);
        TotalDistance = PloggingStats.Distance.year;
        TotalCO2Saved = PloggingStats.CO2Saved.year;
        TotalSteps = PloggingStats.Steps.year;
        TotalWeight = PloggingStats.Weight.year;
        IsBusy= false;
    }
    // Fulfix tills jag lyckas lösa Binding: property not found.
    // Vill bara binda ploggingStats direkt.
    [ObservableProperty]
    string test;
    [ObservableProperty]
    double totalDistance;
    [ObservableProperty]
    double totalCO2Saved;
    [ObservableProperty]
    double totalSteps;
    [ObservableProperty]
    double totalWeight;
    // =======================
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
}
