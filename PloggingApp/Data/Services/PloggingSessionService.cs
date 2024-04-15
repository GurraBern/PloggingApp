using Plogging.Core.Models;
using PloggingApp.Data.Services.ApiClients;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Services;
using RestSharp;

namespace PloggingApp.Data.Services;

public class PloggingSessionService : IPloggingSessionService
{
    private readonly IPloggingApiClient<PloggingSession> _ploggingApiClient;
    private readonly IPloggingImageService _ploggingImageService;
    private readonly IToastService _toastService;

    public PloggingSessionService(IPloggingApiClient<PloggingSession> ploggingApiClient, IPloggingImageService ploggingImageService, IToastService toastService)
    {
        _ploggingApiClient = ploggingApiClient;
        _ploggingImageService = ploggingImageService;
        _toastService = toastService;
    }

    public async Task SavePloggingSession(PloggingSession ploggingSession)
    {
        try
        {
            var ploggingImage = await _ploggingImageService.SaveImage(ploggingSession.Image);

            ploggingSession.Image = ploggingImage.ImageUrl; 
            var request = new RestRequest("api/PloggingSession/UserSessions");
            request.AddBody(ploggingSession);

            await _ploggingApiClient.PostAsync(request);
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
            request.AddParameter("userId", UserId.ToString());
            request.AddParameter("startDate", startDate.ToString("yyyy-MM-ddTHH:mm:ss"));
            request.AddParameter("endDate", endDate.ToString("yyyy-MM-ddTHH:mm:ss"));

            var ploggingSessions = await _ploggingApiClient.GetAllAsync(request);
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
