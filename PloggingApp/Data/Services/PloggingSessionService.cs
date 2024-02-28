using Plogging.Core.Enums;
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

    public async Task<IEnumerable<PloggingSession>> GetUserSessions(DateTime startDate, DateTime endDate, SortProperty sortProperty)
    {
        try
        {
            var request = new RestRequest("api/PloggingSession/Summary");
            request.AddParameter("startDate", startDate);
            request.AddParameter("endDate", endDate);
            request.AddParameter(nameof(SortDirection), SortDirection.Descending);
            request.AddParameter(nameof(SortProperty), sortProperty);

            var ploggingSummaries = await _ploggingApiClient.GetAllAsync(request);


            var sessions = new List<PloggingSession>();
            foreach (var summary in ploggingSummaries)
            {
                var userSession = new PloggingSession()
                {
                    DisplayName = summary.DisplayName,
                    UserId = summary.UserId,
                    PloggingData = summary.PloggingData,

                };

                sessions.Add(userSession);
            }
            return sessions;
        }
        catch (Exception)
        {
            //TODO display toast
            return Enumerable.Empty<PloggingSession>();
        }
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
