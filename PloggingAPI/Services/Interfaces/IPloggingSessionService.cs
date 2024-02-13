using Plogging.Core.Models;
using PloggingAPI.Models.Queries;

namespace PloggingAPI.Services.Interfaces;

public interface IPloggingSessionService
{
    Task<IEnumerable<PloggingSession>> GetSessionSummaries(SessionSummaryQuery query);
    Task<IEnumerable<PloggingSession>> GetPloggingSessions(string userId, DateTime startDate, DateTime endDate);
}