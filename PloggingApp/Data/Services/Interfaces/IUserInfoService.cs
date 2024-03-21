using Plogging.Core.Models;

namespace PloggingApp.Data.Services;

public interface IUserInfoService
{
    Task<UserInfo> GetUser(string userId);
    Task CreateUser(string userId, string displayName);
}

