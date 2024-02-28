
using Plogging.Core.Models;
using System.Collections.ObjectModel;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.Interfaces;

namespace PloggingApp.MVVM.ViewModels;

public partial class OthersSessionsViewModel
{
    public ObservableCollection<PloggingSession> PloggingSessions { get; set; } = [];


    private readonly IPloggingSessionService _ploggingService;



    private IEnumerable<PloggingSession> _allSessesions = new ObservableCollection<PloggingSession>();
    public OthersSessionsViewModel(UserRanking UserRank)
    {



        PloggingSessions.Add(new PloggingSession
        {
            UserId = "user1",
            DisplayName = "Session 1",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddHours(1)
        });

        PloggingSessions.Add(new PloggingSession
        {
            UserId = "user2",
            DisplayName = "Session 2",
            StartDate = DateTime.UtcNow.AddHours(2),
            EndDate = DateTime.UtcNow.AddHours(3)
        }); 
    }

    public async void Hello()
    {
        _allSessesions = await _ploggingService.GetUserSessions();
    }

}

    










    