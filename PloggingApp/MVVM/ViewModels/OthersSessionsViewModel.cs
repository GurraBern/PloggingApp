
using Plogging.Core.Models;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class OthersSessionsViewModel
{
    public ObservableCollection<PloggingSession> PloggingSessions { get; set; } = [];
    public OthersSessionsViewModel()
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

    }

    










    