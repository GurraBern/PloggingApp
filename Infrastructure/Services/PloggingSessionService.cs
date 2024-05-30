using Infrastructure.Interfaces;
using Infrastructure.Services.ApiClients;
using Infrastructure.Services.Interfaces;
using PlogPal.Domain.Models;
using RestSharp;

namespace Infrastructure.Services;

public class PloggingSessionService(IPloggingApiClient<PlogSession> ploggingApiClient, IPloggingImageService ploggingImageService) : IPloggingSessionService
{
    private readonly IPloggingApiClient<PlogSession> _ploggingApiClient = ploggingApiClient;
    private readonly IPloggingImageService _ploggingImageService = ploggingImageService;

    public async Task SavePloggingSession(PlogSession ploggingSession)
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
            //await _toastService.MakeToast("Could not save plogging session");
        }
    }

    public async Task<IEnumerable<PlogSession>> GetUserSessions(string userId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var request = new RestRequest("api/PloggingSession/UserSessions");
            request.AddParameter("userId", userId);
            request.AddParameter("startDate", startDate);
            request.AddParameter("endDate", endDate);

            var ploggingSessions = await _ploggingApiClient.GetAllAsync(request);
            return ploggingSessions;
        }
        catch (Exception)
        {
            //await _toastService.MakeToast("Could not fetch user sessions");

            return [];
        }
    }
}
