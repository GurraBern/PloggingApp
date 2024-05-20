using Plogging.Core.Models;
using PloggingAPI.Features.LitterLocations;

namespace PloggingAPI.Features.PloggingSession;

public class PloggingSessionService : IPloggingSessionService
{
    private readonly IPloggingSessionRepository _ploggingSessionRepository;
    private readonly ILitterLocationsRepository _litterLocationsRepository;

    public PloggingSessionService(IPloggingSessionRepository ploggingSessionRepository, ILitterLocationsRepository litterLocationsRepository)
    {
        _ploggingSessionRepository = ploggingSessionRepository;
        _litterLocationsRepository = litterLocationsRepository;
    }

    public async Task<IEnumerable<PlogSession>> GetSessionSummaries(SessionSummaryQuery query)
    {
        var sessions = await _ploggingSessionRepository.GetSessionSummaries(query);
        return sessions;
    }

    public async Task<IEnumerable<PlogSession>> GetPloggingSessions(string userId, DateTime startDate, DateTime endDate)
    {
        var sessions = await _ploggingSessionRepository.GetPloggingSessions(userId, startDate, endDate);

        return sessions;
    }

    public async Task<IEnumerable<LitterLocation>> GetLitterLocations()
    {
        var litterLocations = await _litterLocationsRepository.GetLitterLocations();
        return litterLocations;
    }

    public async Task AddPloggingSession(PlogSession ploggingSession)
    {
        await _ploggingSessionRepository.InsertPloggingSession(ploggingSession);

        var litterLocations = LitterConverter.ToLitterLocations(ploggingSession.PloggingData.Litters);
        await _litterLocationsRepository.InsertLitterLocations(litterLocations);
    }
}
