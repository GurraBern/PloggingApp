using Plogging.Core.Models;
using PloggingApp.Services.Authentication;
using PloggingApp.Shared;
using RestSharp;

namespace PloggingApp.Data.Services.Interfaces;

public class LitterLocationService : ILitterLocationService
{
    private readonly IPloggingApiClient<LitterLocation> _ploggingApiClient;
    private readonly IAuthenticationService _authenticationService;

    public LitterLocationService(IPloggingApiClient<LitterLocation> ploggingApiClient, IAuthenticationService authenticationService)
    {
        _ploggingApiClient = ploggingApiClient;
        _authenticationService = authenticationService;
    }

    public async Task<IEnumerable<LitterLocation>> GetLitterLocations()
    {
        var request = new RestRequest("api/PloggingSession/LitterLocations");

        var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
        var litterLocations = await _ploggingApiClient.GetAllAsync(request, bearerToken);

        return litterLocations;
    }
}
