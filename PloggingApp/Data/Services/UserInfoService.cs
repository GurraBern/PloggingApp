using Plogging.Core.Models;
using PloggingApp.Services.Authentication;
using PloggingApp.Shared;
using RestSharp;

namespace PloggingApp.Data.Services;

public class UserInfoService : IUserInfoService
{
    private readonly IPloggingApiClient<UserInfo> _ploggingApiClient;
    private readonly IAuthenticationService _authenticationService;

    public UserInfoService(IPloggingApiClient<UserInfo> ploggingApiClient, IAuthenticationService authenticationService)
	{
        _ploggingApiClient = ploggingApiClient;
        _authenticationService = authenticationService;
    }

    public async Task<UserInfo> GetUser(string userId)
    {
        try
        {
            var request = new RestRequest($"api/UserInfo/GetUserInfo/{userId}");

            var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
            var user = await _ploggingApiClient.GetAsync(request, bearerToken);
            return user;
        }
        catch (Exception ex)
        {
            //TODO display toast
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }

    public async Task CreateUser(string userId, string displayName)
    {
        UserInfo user = new UserInfo
        {
            UserId = userId,
            DisplayName = displayName
        };

        try
        {
            var request = new RestRequest("api/UserInfo/CreateUser");
            request.AddBody(user);

            var bearerToken = _authenticationService.CurrentUser.Credential.IdToken;
            await _ploggingApiClient.PostAsync(request, bearerToken);
        }
        catch (Exception ex)
        {
            //TODO display toast
            Console.WriteLine($"An error occured: {ex.Message}");
        }
    }
}

