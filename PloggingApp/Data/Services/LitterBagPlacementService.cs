using Plogging.Core.Models;
using PloggingApp.Data.Services.ApiClients;
using PloggingApp.Data.Services.Interfaces;
using RestSharp;

namespace PloggingApp.Data.Services;

public class LitterBagPlacementService : ILitterBagPlacementService
{
    private readonly IPloggingApiClient<LitterBagPlacement> _ploggingApiClient;

    public LitterBagPlacementService(IPloggingApiClient<LitterBagPlacement> ploggingApiClient)
    {
        _ploggingApiClient = ploggingApiClient;
    }

    public async Task AddTrashCollectionPoint(LitterBagPlacement litterBagPlacement)
    {
        var request = new RestRequest("api/LitterBagPlacement");
        request.AddBody(litterBagPlacement);

        await _ploggingApiClient.PostAsync(request, ""); 
    }

    public async Task<IEnumerable<LitterBagPlacement>> GetLitterBagPlacements()
    {
        var request = new RestRequest("api/LitterBagPlacement");

        var litterBagPlacements = await _ploggingApiClient.GetAllAsync(request);

        return litterBagPlacements;
    }
}
