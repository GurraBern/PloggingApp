using Plogging.Core.Models;

namespace PloggingAPI.Repository.Interfaces;

public interface IUserInfoRepository
{
    Task CreateUser(UserInfo user);
    Task DeleteUser(string userId);
    Task<UserInfo> GetUser(string userId);
}
