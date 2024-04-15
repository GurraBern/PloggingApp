using PloggingApp.Pages;

using Plogging.Core.Models;
using System.Collections.ObjectModel;
using PloggingApp.Services;
using PloggingApp.Services.SessionStatistics;


namespace PloggingApp.Services.Authentication;

public class SessionStatisticsService : ISessionStatisticsService
{
    public async Task GoToSessionStats(PloggingSession session)
    {
        if (session is null)
            return;
        await Shell.Current.GoToAsync($"{nameof(SessionStatisticsPage)}", true,
            new Dictionary<string, object>
            {
                {nameof(PloggingSession), session}
            });
    }

}
