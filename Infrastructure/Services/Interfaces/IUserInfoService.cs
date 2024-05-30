using PlogPal.Domain.Models;

namespace Infrastructure.Services.Interfaces;

public interface IUserInfoService
{
    Task<UserInfo> GetUser(string userId);
    Task CreateUser(string userId, string displayName);
}

