using Plogging.Core.Models;
using PloggingApp.Data.Services.ApiClients;
using RestSharp;

namespace PloggingApp.Data.Services;

public class StreakService : IStreakService
{
    private readonly IPloggingApiClient<UserStreak> _ploggingApiClient;

    public StreakService(IPloggingApiClient<UserStreak> ploggingApiClient)
	{
        _ploggingApiClient = ploggingApiClient;
	}

    public async Task<UserStreak> GetUserStreak(string id)
    {
        try
        {
            var request = new RestRequest("api/Streak/GetUserStreak");
            request.AddParameter("id", id);

            var user = await _ploggingApiClient.GetAsync(request);
            return user;
        }
        catch (Exception)
        {
            //TODO display toast
            return null;
        }
    }
}


