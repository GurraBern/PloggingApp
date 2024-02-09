using Plogging.Core.Models;

namespace PloggingAPI.Services.Interfaces;

public interface IPloggingSessionService
{
    Task<IEnumerable<PloggingSession>> GetSessionSummaries();
}