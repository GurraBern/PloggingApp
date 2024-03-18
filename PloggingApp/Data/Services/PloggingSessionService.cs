using Plogging.Core.Models;
using PloggingApp.Data.Services.ApiClients;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Services;
using RestSharp;

namespace PloggingApp.Data.Services;

public class PloggingSessionService : IPloggingSessionService
{
    private readonly IPloggingApiClient<PloggingSession> _ploggingApiClient;
    private readonly IToastService _toastService;

    public PloggingSessionService(IPloggingApiClient<PloggingSession> ploggingApiClient, IToastService toastService)
    {
        _ploggingApiClient = ploggingApiClient;
        _toastService = toastService;
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
            await _toastService.MakeToast("Could not save plogging session");
        }
    }

    public async Task<IEnumerable<PloggingSession>> GetUserSessions(string UserId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var request = new RestRequest("api/PloggingSession/UserSessions");
            request.AddParameter("userId", UserId);
            request.AddParameter("startDate", startDate);
            request.AddParameter("endDate", endDate);

            var ploggingSessions = await _ploggingApiClient.GetAllAsync(request);
            return ploggingSessions;
        }
        catch (Exception)
        {
            await _toastService.MakeToast("Could not fetch user sessions");

            return Enumerable.Empty<PloggingSession>();
        }
    }
}
