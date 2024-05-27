using PlogPal.Domain.Models;

namespace Infrastructure.Services.Interfaces;

public interface IPlogTogetherService
{
    Task AddUserToGroup(string ownerUserId, string userId);

    Task DeleteGroup(string ownerUserId);

    Task<PlogTogether> GetPlogTogether(string ownerUserId);

    Task LeaveGroup(string userId);
}