﻿using Plogging.Core.Models;
using PloggingApp.Data.Services.ApiClients;
using PloggingApp.Data.Services.Interfaces;
using RestSharp;

namespace PloggingApp.Data.Services;

public class LitterbagPlacementService : ILitterbagPlacementService
{
    private readonly IPloggingApiClient<LitterbagPlacement> _ploggingApiClient;

    public LitterbagPlacementService(IPloggingApiClient<LitterbagPlacement> ploggingApiClient)
    {
        _ploggingApiClient = ploggingApiClient;
    }

    public async Task AddTrashCollectionPoint(LitterbagPlacement litterbagPlacement)
    {
        var request = new RestRequest("api/LitterbagPlacement");
        request.AddBody(litterbagPlacement);

        await _ploggingApiClient.PostAsync(request, ""); 
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
