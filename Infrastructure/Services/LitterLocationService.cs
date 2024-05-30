using Infrastructure.Services.ApiClients;
using PlogPal.Application.Common.Interfaces;
using PlogPal.Domain.Models;
using RestSharp;

namespace Infrastructure.Services;

public class LitterLocationService(IPloggingApiClient<LitterLocation> ploggingApiClient) : ILitterLocationService
{
    private readonly IPloggingApiClient<LitterLocation> _ploggingApiClient = ploggingApiClient;

    public async Task<IEnumerable<LitterLocation>> GetLitterLocations()
    {
        var request = new RestRequest("api/PloggingSession/LitterLocations");

        var litterLocations = await _ploggingApiClient.GetAllAsync(request);

        return litterLocations;
    }
}
