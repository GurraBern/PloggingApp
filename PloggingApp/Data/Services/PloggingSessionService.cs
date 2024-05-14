﻿using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.Services;
using PloggingApp.Services.Authentication;
using PloggingApp.Shared;
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

            await _ploggingApiClient.PostAsync(request, _authenticationService.BearerToken);
        }
        catch (Exception ex)
        {
            await _toastService.MakeToast("Could not save plogging session");
        }
    }

    public async Task<IEnumerable<PloggingSession>> GetUserSessions(string userId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var request = new RestRequest("api/PloggingSession/UserSessions");
            request.AddParameter("userId", userId);
            request.AddParameter("startDate", startDate);
            request.AddParameter("endDate", endDate);

            var ploggingSessions = await _ploggingApiClient.GetAllAsync(request, _authenticationService.BearerToken);
            return ploggingSessions;
        }
        catch (Exception)
        {
            await _toastService.MakeToast("Could not fetch user sessions");

            return [];
        }
    }
    public string UserId { get; set; }
    public string MyUserId { get; set; }

    public string OtherUserId { get; set; }
    //public string SessionId { get; set; }
}
