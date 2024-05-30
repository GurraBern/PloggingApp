using Infrastructure.Services.ApiClients;
using Infrastructure.Services.Interfaces;
using PlogPal.Application.Common.Interfaces;
using PlogPal.Domain.Models;
using RestSharp;

namespace Infrastructure.Services;

public class StreakService : IStreakService
{
    private readonly IPloggingApiClient<UserStreak> _ploggingApiClient;
    private readonly IUserContext _userContext;

    public StreakService(IPloggingApiClient<UserStreak> ploggingApiClient, IUserContext userContext)
    {
        _ploggingApiClient = ploggingApiClient;
        _userContext = userContext;
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

    public async Task ResetStreak()
    {
        try
        {
            var request = new RestRequest($"api/Streak/ResetStreak/{_userContext.UserId}");

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
            var request = new RestRequest($"api/Streak/UpdateStreak/{userId}");

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
        UserStreak user = new UserStreak
        {
            UserId = userId,
            Streak = 0,
            BiggestStreak = 0
        };

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

