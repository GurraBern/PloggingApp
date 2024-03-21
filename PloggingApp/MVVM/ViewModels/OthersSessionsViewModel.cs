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
    public ObservableObject _PloggingStatistics{ get; set; }
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
        _PloggingStatistics = new PloggingStatistics(_allSessions);
        
        PloggingSessions.ClearAndAddRange(_allSessions);
        IsBusy = false;
    }




}