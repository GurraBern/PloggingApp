using Infrastructure.Services.ApiClients;
using Infrastructure.Services.Interfaces;
using PlogPal.Domain.Models;
using RestSharp;

namespace Infrastructure.Services;

public class PlogTogetherService(IPloggingApiClient<PlogTogether> ploggingApiClient) : IPlogTogetherService
{
    private readonly IPloggingApiClient<PlogTogether> _ploggingApiClient = ploggingApiClient;

    public async Task AddUserToGroup(string ownerUserId, string userId)
    {
        try
        {
            var request = new RestRequest($"api/PlogTogether/AddUserToGroup/{ownerUserId}/{userId}");

            await _ploggingApiClient.PostAsync(request);
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

            await _ploggingApiClient.DeleteAsync(request);
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

            var plogTogether = await _ploggingApiClient.GetAsync(request);
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

            await _ploggingApiClient.PutAsync(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

