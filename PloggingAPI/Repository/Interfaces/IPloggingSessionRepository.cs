using Plogging.Core.Models;

namespace PloggingAPI.Repository.Interfaces;

public interface IPloggingSessionRepository
{
    Task<IEnumerable<PloggingSession>> GetSessions();
    Task<IEnumerable<PloggingSession>> GetSessionSummaries();
}