using Plogging.Core.Models;
using PloggingApp.Data.Services.ApiClients;
using PloggingApp.Data.Services.Interfaces;
using RestSharp;

namespace PloggingApp.Data.Services;

public class LitterbagPlacementService : ILitterbagPlacementService
{
    private readonly IPloggingApiClient<LitterbagPlacement> _ploggingApiClient;
    private readonly IPloggingImageService _ploggingImageService;

    public LitterbagPlacementService(IPloggingApiClient<LitterbagPlacement> ploggingApiClient, IPloggingImageService ploggingImageService)
    {
        _ploggingApiClient = ploggingApiClient;
        _ploggingImageService = ploggingImageService;
    }

    public async Task AddTrashCollectionPoint(LitterbagPlacement litterbagPlacement)
    {
        litterbagPlacement = await SaveLitterbagImage(litterbagPlacement);

        var placementRequest = new RestRequest("api/LitterbagPlacement");
        placementRequest.AddBody(litterbagPlacement);

        await _ploggingApiClient.PostAsync(placementRequest);
    }

    private async Task<LitterbagPlacement> SaveLitterbagImage(LitterbagPlacement litterbagPlacement)
    {
        var ploggingImage = await _ploggingImageService.SaveImage(litterbagPlacement.ImageUrl);

        litterbagPlacement.ImageUrl = ploggingImage.ImageUrl;

        return litterbagPlacement;
    }

    public async Task CollectLitterbagPlacement(string id, int distance)
    {
        var request = new RestRequest("api/LitterbagPlacement");
        request.AddParameter("litterbagPlacementId", id);
        request.AddParameter("distanceToLitterbag", distance);

        await _ploggingApiClient.DeleteAsync(request);
    }

    public async Task<IEnumerable<LitterbagPlacement>> GetLitterbagPlacements()
    {
        var request = new RestRequest("api/LitterbagPlacement");

        var litterbagPlacements = await _ploggingApiClient.GetAllAsync(request);

        return litterbagPlacements;
    }
}
