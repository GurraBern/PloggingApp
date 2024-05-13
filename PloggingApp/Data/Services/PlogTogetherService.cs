using Plogging.Core.Models;
using PloggingApp.Services.Authentication;
using PloggingApp.Shared;
using RestSharp;

namespace PloggingApp.Data.Services;

public class PlogTogetherService : IPlogTogetherService
{
    private readonly IPloggingApiClient<PlogTogether> _ploggingApiClient;
    private readonly IAuthenticationService _authenticationService;

    public PlogTogetherService(IPloggingApiClient<PlogTogether> ploggingApiClient, IAuthenticationService authenticationService)
	{
        _ploggingApiClient = ploggingApiClient;
        _authenticationService = authenticationService;
    }

    public async Task AddUserToGroup(string ownerUserId, string userId)
    {
        try
        {
            var request = new RestRequest($"api/PlogTogether/AddUserToGroup/{ownerUserId}/{userId}");

            var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
            await _ploggingApiClient.PostAsync(request, bearerToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }

    public async Task DeleteGroup(string ownerUserId)
    {
        try
        {
            var request = new RestRequest("api/PlogTogether/DeleteGroup");
            request.AddParameter("ownerUserId", ownerUserId);

            var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
            await _ploggingApiClient.DeleteAsync(request, bearerToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public async Task<PlogTogether> GetPlogTogether(string ownerUserId)
    {
        try
        {
            var request = new RestRequest($"api/PlogTogether/GetPlogTogether/{ownerUserId}");

            var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
            var plogTogether = await _ploggingApiClient.GetAsync(request, bearerToken);
            return plogTogether;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }

    public async Task LeaveGroup(string userId)
    {
        try
        {
            var request = new RestRequest($"api/PlogTogether/LeaveGroup/{userId}");

            var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
            await _ploggingApiClient.PutAsync(request, bearerToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

