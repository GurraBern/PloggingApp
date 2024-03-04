using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using SkiaSharp;
using Plogging.Core.Models;
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
        TrashTypeChart = statisticsService.generateLitterGraph(Plogging.Core.Enums.TimeResolution.ThisYear);
        IsBusy = false;
    }
    [RelayCommand]
    private async Task Test()
    {
        await Shell.Current.DisplayAlert("Test", "TEST", "OK");
    }
    [ObservableProperty]
    Chart trashTypeChart;
    [ObservableProperty]
    double totalDistance;
}
