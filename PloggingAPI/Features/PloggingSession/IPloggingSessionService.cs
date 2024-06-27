using PlogPal.Domain.Models;

namespace PloggingAPI.Features.PloggingSession;

public interface IPloggingSessionService
{
    Task<IEnumerable<PlogSession>> GetSessionSummaries(SessionSummaryQuery query);
    Task<IEnumerable<PlogSession>> GetPloggingSessions(string userId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<LitterLocation>> GetLitterLocations();
    Task AddPloggingSession(PlogSession ploggingSession);
}