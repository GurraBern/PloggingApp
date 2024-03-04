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

namespace PloggingApp.MVVM.ViewModels;
public partial class StatisticsViewModel : BaseViewModel, IAsyncInitialization
{
    public Task Initialization { get; private set; }

    private readonly IPloggingSessionService _ploggingSessionService;
    private IStatisticsService statisticsService;
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
        _allUserSessions = await _ploggingSessionService.GetUserSessions("TODOsetUserId", new DateTime(2024, 3, 2), DateTime.UtcNow);
        UserSessions.ClearAndAddRange(_allUserSessions);
        statisticsService = new StatisticsService(UserSessions);
        StepsChart = statisticsService.generateStepsChart(TimeResolution.ThisYear);
        DistanceChart = statisticsService.generateDistanceChart(TimeResolution.ThisYear);
        IsBusy = false;
    }
    [RelayCommand]
    private async Task GetMonthChart()
    {
        IsBusy = true;
        StepsChart = statisticsService.generateStepsChart(TimeResolution.ThisMonth);
        DistanceChart = statisticsService.generateDistanceChart(TimeResolution.ThisMonth);
        IsBusy = false;
    }
    [ObservableProperty]
    Chart stepsChart;
    [ObservableProperty]
    Chart distanceChart;

 
    [ObservableProperty]
    double totalDistance;
}
