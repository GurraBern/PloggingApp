using Plogging.Core.Models;

namespace PloggingApp.Data.Services.Interfaces
{
    public interface IPloggingSessionService
    {
        Task SavePloggingSession(PlogSession ploggingSession);
        Task<IEnumerable<PlogSession>> GetUserSessions(string userId, DateTime startDate, DateTime endDate);

        // string UserId { get; set; }
        // string OtherUserId { get; set; }
        // string MyUserId { get; set; }
        //string SessionId { get; set; }
    }
}