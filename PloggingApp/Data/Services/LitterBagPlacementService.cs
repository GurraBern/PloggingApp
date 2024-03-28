using Plogging.Core.Models;
using PloggingApp.Data.Services.ApiClients;
using PloggingApp.Data.Services.Interfaces;
using RestSharp;

namespace PloggingApp.Data.Services;

public class LitterbagPlacementService : ILitterbagPlacementService
{
    private readonly IPloggingApiClient<LitterbagPlacement> _ploggingApiClient;
    private readonly IPloggingApiClient<PloggingImage> _imageApiClient;

    public LitterbagPlacementService(IPloggingApiClient<LitterbagPlacement> ploggingApiClient, IPloggingApiClient<PloggingImage> imageApiClient)
    {
        _ploggingApiClient = ploggingApiClient;
        _imageApiClient = imageApiClient;
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
        var imageRequest = new RestRequest("api/Image");
        imageRequest.AddFile("image", litterbagPlacement.ImageUrl);

        var response = await _imageApiClient.PostAsync(imageRequest);

        litterbagPlacement.ImageUrl = response.ImageUrl;

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
