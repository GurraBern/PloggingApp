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
            request.AddParameter("userId", id);

            var user = await _ploggingApiClient.GetAsync(request);
            return user;
        }
        catch (Exception ex)
        {
            //TODO display toast
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }


    //make them return UserStreak?
    public async Task ResetStreak(string userId)
    {
        try
        {
            var request = new RestRequest("api/Streak/ResetSreak");
            request.AddParameter("userId", userId);

            await _ploggingApiClient.PatchAsync(request);
        }
        catch (Exception ex)
        {
            //TODO display toast
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public async Task UpdateStreak(string userId)
    {
        try
        {
            var request = new RestRequest("api/Streak/UpdateSreak");
            request.AddParameter("userId", userId);

            await _ploggingApiClient.PatchAsync(request);
        }
        catch (Exception ex)
        {
            //TODO display toast
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public async Task CreateUser(string userId)
    {
        UserStreak user = new UserStreak {
            UserId = userId,
            Streak = 0};

        try
        {
            var request = new RestRequest("api/Streak/CreateUser");
            request.AddBody(user);

            await _ploggingApiClient.PostAsync(request);
        }
        catch (Exception ex)
        {
            //TODO display toast
            Console.WriteLine($"An error occured: {ex.Message}");
        }
    }
}


