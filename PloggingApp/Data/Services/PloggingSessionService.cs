using Plogging.Core.Models;
using PloggingApp.Data.Services.ApiClients;
using PloggingApp.Data.Services.Interfaces;
using RestSharp;

namespace PloggingApp.Data.Services;

public class PloggingSessionService : IPloggingSessionService
{
    private readonly IPloggingApiClient<PloggingSession> _ploggingApiClient;

    public PloggingSessionService(IPloggingApiClient<PloggingSession> ploggingApiClient)
    {
        _ploggingApiClient = ploggingApiClient;
    }

    public async Task SavePloggingSession(PloggingSession ploggingSession)
    {
        try
        {
            var request = new RestRequest("api/PloggingSession/UserSessions");
            request.AddBody(ploggingSession);

            await _ploggingApiClient.PostAsync(request);
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}
