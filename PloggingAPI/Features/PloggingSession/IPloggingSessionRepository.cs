using Plogging.Core.Models;

namespace PloggingAPI.Features.PloggingSession;

public interface IPloggingSessionRepository
{
    Task InsertPloggingSession(PlogSession ploggingSession);
    Task<IEnumerable<PlogSession>> GetPloggingSessions(string userId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<PlogSession>> GetSessionSummaries(SessionSummaryQuery query);
}