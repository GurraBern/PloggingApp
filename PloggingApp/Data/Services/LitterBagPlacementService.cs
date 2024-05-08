using Plogging.Core.Models;
using PloggingApp.Data.Services.ApiClients;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Services.Authentication;
using RestSharp;

namespace PloggingApp.Data.Services;

public class LitterbagPlacementService : ILitterbagPlacementService
{
    private readonly IPloggingApiClient<LitterbagPlacement> _ploggingApiClient;
    private readonly IPloggingImageService _ploggingImageService;
    private readonly IAuthenticationService _authenticationService;

    public LitterbagPlacementService(IPloggingApiClient<LitterbagPlacement> ploggingApiClient, IPloggingImageService ploggingImageService, IAuthenticationService authenticationService)
    {
        _ploggingApiClient = ploggingApiClient;
        _ploggingImageService = ploggingImageService;
        _authenticationService = authenticationService;
    }

    public async Task AddTrashCollectionPoint(LitterbagPlacement litterbagPlacement)
    {
        litterbagPlacement = await SaveLitterbagImage(litterbagPlacement);

        var placementRequest = new RestRequest("api/LitterbagPlacement");
        placementRequest.AddBody(litterbagPlacement);

        var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
        await _ploggingApiClient.PostAsync(placementRequest, bearerToken);
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

        var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
        await _ploggingApiClient.DeleteAsync(request, bearerToken);
    }

    public async Task<IEnumerable<LitterbagPlacement>> GetLitterbagPlacements()
    {
        var request = new RestRequest("api/LitterbagPlacement");

        var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
        var litterbagPlacements = await _ploggingApiClient.GetAllAsync(request, bearerToken);

        return litterbagPlacements;
    }
}
