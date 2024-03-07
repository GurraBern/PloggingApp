using Plogging.Core.Models;
using PloggingApp.Data.Services.ApiClients;
using RestSharp;

namespace PloggingApp.Data.Services;

public class PlogTogetherService : IPlogTogetherService
{
    private readonly IPloggingApiClient<PlogTogether> _ploggingApiClient;

    public PlogTogetherService(IPloggingApiClient<PlogTogether> ploggingApiClient)
	{
        _ploggingApiClient = ploggingApiClient;
	}

    // Post, patch or put?
    public async Task AddUserToGroup(string ownerUserId, string userId)
    {
        try
        {
            var request = new RestRequest("api/PlogTogether/AddUserToGroup");
            request.AddParameter("ownerUserId", ownerUserId);
            request.AddParameter("addUserId", userId);

            await _ploggingApiClient.PostAsync(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }

    // change to DeleteAsync when in main
    public async Task DeleteGroup(string ownerUserId)
    {
        try
        {
            var request = new RestRequest("api/PlogTogether/DeleteGroup");
            request.AddParameter("ownerUserId", ownerUserId);

            await _ploggingApiClient.PostAsync(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

