using Plogging.Core.Models;
using PloggingApp.Data.Services.ApiClients;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Services;
using PloggingApp.Services.Authentication;
using RestSharp;

namespace PloggingApp.Data.Services;

public class PloggingSessionService : IPloggingSessionService
{
    private readonly IPloggingApiClient<PloggingSession> _ploggingApiClient;
    private readonly IPloggingImageService _ploggingImageService;
    private readonly IToastService _toastService;
    private readonly IAuthenticationService _authenticationService;

    public PloggingSessionService(IPloggingApiClient<PloggingSession> ploggingApiClient, IPloggingImageService ploggingImageService, IToastService toastService, IAuthenticationService authenticationService)
    {
        _ploggingApiClient = ploggingApiClient;
        _ploggingImageService = ploggingImageService;
        _toastService = toastService;
        _authenticationService = authenticationService;
    }

    public async Task SavePloggingSession(PloggingSession ploggingSession)
    {
        try
        {
            var ploggingImage = await _ploggingImageService.SaveImage(ploggingSession.Image);

            ploggingSession.Image = ploggingImage.ImageUrl; 
            var request = new RestRequest("api/PloggingSession/UserSessions");
            request.AddBody(ploggingSession);

            var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
            await _ploggingApiClient.PostAsync(request, bearerToken);
        }
        catch (Exception ex)
        {
            await _toastService.MakeToast("Could not save plogging session");
        }
    }

    public async Task<IEnumerable<PloggingSession>> GetUserSessions(string UserId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var request = new RestRequest("api/PloggingSession/UserSessions");
            request.AddParameter("userId", UserId);
            request.AddParameter("startDate", startDate);
            request.AddParameter("endDate", endDate);

            var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
            var ploggingSessions = await _ploggingApiClient.GetAllAsync(request, bearerToken);
            return ploggingSessions;
        }
        catch (Exception)
        {
            await _toastService.MakeToast("Could not fetch user sessions");

            return Enumerable.Empty<PloggingSession>();
        }
    }
    public string UserId { get; set; }
    public string MyUserId { get; set; }

    public string OtherUserId { get; set; }
    public string SessionId { get; set; }
}
