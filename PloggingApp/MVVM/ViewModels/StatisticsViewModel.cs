using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using SkiaSharp;
using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Extensions;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;
public partial class StatisticsViewModel : BaseViewModel, IAsyncInitialization
{
    // Hardcoded some entries to test
    ChartEntry[] trashEntries =
    [
        new ChartEntry(1)
     {
         Label = "Aluminium",
         ValueLabel = "27",
         Color = SKColor.Parse("#ffd86e")
     },
     new ChartEntry(2)
     {
         Label = "Plastic",
         ValueLabel = "55",
         Color = SKColor.Parse("#7aff6e")
     },
     new ChartEntry(3)
     {
         Label = "Other",
         ValueLabel = "14",
         Color = SKColor.Parse("#6e84ff")
     },
 ];

    public Task Initialization { get; private set; }
    private readonly IPloggingSessionService _ploggingSessionService;
    public ObservableCollection<PloggingSession> UserSessions { get; set; } = [];
    private IEnumerable<PloggingSession> _allUserSessions = new ObservableCollection<PloggingSession>();
    public StatisticsViewModel(IPloggingSessionService ploggingSessionService)
    {
        TrashTypeChart = new DonutChart()
        {
            LabelTextSize = 40,
            Entries = trashEntries
        };
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
        foreach (PloggingSession sesh in _allUserSessions)
        {
            TotalDistance += sesh.PloggingData.Distance;
        }
        IsBusy = false;
    }

    [ObservableProperty]
    Chart trashTypeChart;
    [ObservableProperty]
    double totalDistance;
}
