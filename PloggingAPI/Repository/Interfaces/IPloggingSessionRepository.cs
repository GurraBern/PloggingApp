using Plogging.Core.Models;
using PloggingAPI.Models.Queries;

namespace PloggingAPI.Repository.Interfaces;

public interface IPloggingSessionRepository
{
    Task InsertPloggingSession(PloggingSession ploggingSession);
    Task<IEnumerable<PloggingSession>> GetPloggingSessions(string userId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<PloggingSession>> GetSessionSummaries(SessionSummaryQuery query);
}