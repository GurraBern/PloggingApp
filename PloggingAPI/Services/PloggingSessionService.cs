using Plogging.Core.Models;
using PloggingAPI.Repository.Interfaces;
using PloggingAPI.Services.Interfaces;

namespace PloggingAPI.Services;

public class PloggingSessionService : IPloggingSessionService
{
    private readonly IPloggingSessionRepository _ploggingSessionRepository;

    public PloggingSessionService(IPloggingSessionRepository ploggingSessionRepository)
    {
        _ploggingSessionRepository = ploggingSessionRepository;
    }

    public async Task<IEnumerable<PloggingSession>> GetSessionSummaries()
    {
        var sessions = await _ploggingSessionRepository.GetSessionSummaries();
        return sessions;
    }
}
