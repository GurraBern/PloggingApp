using Plogging.Core.Models;
using PloggingApp.Services.Authentication;
using PloggingApp.Shared;
using RestSharp;

namespace PloggingApp.Data.Services;

public class StreakService : IStreakService
{
    private readonly IPloggingApiClient<UserStreak> _ploggingApiClient;
    private readonly IAuthenticationService _authenticationService;

    public StreakService(IPloggingApiClient<UserStreak> ploggingApiClient, IAuthenticationService authenticationService)
	{
        _ploggingApiClient = ploggingApiClient;
        _authenticationService = authenticationService;
    }

    public async Task<UserStreak> GetUserStreak(string id)
    {
        try
        {
            var request = new RestRequest("api/Streak/GetUserStreak");
            request.AddParameter("userId", id);

            var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
            var user = await _ploggingApiClient.GetAsync(request, bearerToken);
            return user;
        }
        catch (Exception ex)
        {
            //TODO display toast
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }

    public async Task ResetStreak(string userId)
    {
        try
        {
            var request = new RestRequest($"api/Streak/ResetStreak/{userId}");

            var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
            await _ploggingApiClient.PatchAsync(request, bearerToken);
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

            var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
            await _ploggingApiClient.PatchAsync(request, bearerToken);
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
            Streak = 0,
            BiggestStreak = 0};

        try
        {
            var request = new RestRequest("api/Streak/CreateUser");
            request.AddBody(user);

            var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
            await _ploggingApiClient.PostAsync(request, bearerToken);
        }
        catch (Exception ex)
        {
            //TODO display toast
            Console.WriteLine($"An error occured: {ex.Message}");
        }
    }
}

