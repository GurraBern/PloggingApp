using Plogging.Core.Models;

namespace PloggingApp.Services.SessionStatistics;

public interface ISessionStatisticsService
{    
    Task GoToSessionStats(PloggingSession session);
}