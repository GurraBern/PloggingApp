using Plogging.Core.Models;
using PloggingApp.Services.Authentication;
using PloggingApp.Shared;
using RestSharp;

namespace PloggingApp.Data.Services;

public class UserInfoService(IPloggingApiClient<UserInfo> ploggingApiClient) : IUserInfoService
{
    private readonly IPloggingApiClient<UserInfo> _ploggingApiClient = ploggingApiClient;

    public async Task<UserInfo> GetUser(string userId)
    {
        try
        {
            var request = new RestRequest($"api/UserInfo/GetUserInfo/{userId}");

            var user = await _ploggingApiClient.GetAsync(request);
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

            await _ploggingApiClient.PostAsync(request);
        }
        catch (Exception ex)
        {
            //TODO display toast
            Console.WriteLine($"An error occured: {ex.Message}");
        }
    }
}
