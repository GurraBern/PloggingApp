using Plogging.Core.Models;
using PloggingApp.Shared;
using RestSharp;

namespace PloggingApp.Data.Services.Interfaces;

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
