using Plogging.Core.Models;

namespace PloggingApp.Data.Services.Interfaces
{
    public interface IPloggingSessionService
    {
        Task SavePloggingSession(PloggingSession ploggingSession);
        Task<IEnumerable<PloggingSession>> GetUserSessions(string UserId, DateTime startDate, DateTime endDate);
        string UserId { get; set; }
        string OtherUserId { get; set; }
        string MyUserId { get; set; }
        string SessionId { get; set; }

        
    }
}