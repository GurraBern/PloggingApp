using Plogging.Core.Models;
using System.Collections.ObjectModel;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Extensions;
using CommunityToolkit.Mvvm.Input;

using Plogging.Core.Enums;
using PloggingApp.MVVM.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class OthersSessionsViewModel : BaseViewModel, IAsyncInitialization
{

    public ObservableCollection<PloggingSession> PloggingSessions { get; set; } = [];

    [ObservableProperty]
    public double totalSteps;
    [ObservableProperty]
    public double totalDistance;
    [ObservableProperty]
    public double totalCO2Saved;
    [ObservableProperty]
    public double totalWeight;


    private IEnumerable<PloggingSession> _allSessions = new ObservableCollection<PloggingSession>();

    private readonly IPloggingSessionService _sessionService;
    private IRelayCommand? RecentSessionCommand { get; set; }
    public Task Initialization { get; private set; }

    public OthersSessionsViewModel(IPloggingSessionService SessionService)
    {
        _sessionService = SessionService;
        Initialization = GetSessions();



    }

    [RelayCommand]
    public async Task UpdatePage()
    {

        GetSessions();

    }

    public async Task GetSessions()
    {
        IsBusy = true;
        var test = _sessionService.UserId;
        _allSessions = await _sessionService.GetUserSessions(test, DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
        var stats = new PloggingStatistics(_allSessions);
        totalSteps = stats.TotalSteps;
        totalDistance = stats.TotalDistance;
        totalCO2Saved = stats.TotalCO2Saved;
        totalWeight = stats.TotalWeight;
        PloggingSessions.ClearAndAddRange(_allSessions);
        IsBusy = false;
    }




}