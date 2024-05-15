using Plogging.Core.Models;
using PloggingApp.Services.Authentication;
using PloggingApp.Shared;
using RestSharp;

namespace PloggingApp.Data.Services.Interfaces;

public class LitterLocationService : ILitterLocationService
{
    private readonly IPloggingApiClient<LitterLocation> _ploggingApiClient;

    public LitterLocationService(IPloggingApiClient<LitterLocation> ploggingApiClient)
    {
        _ploggingApiClient = ploggingApiClient;
    }

    public async Task<IEnumerable<LitterLocation>> GetLitterLocations()
    {
        var request = new RestRequest("api/PloggingSession/LitterLocations");

        var litterLocations = await _ploggingApiClient.GetAllAsync(request);

        return litterLocations;
    }
}
