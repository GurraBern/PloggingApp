using Plogging.Core.Models;

namespace PloggingAPI.Repository.Interfaces;

public interface IPlogTogetherRepository
{
    Task AddUserToGroup(string ownerUserId, string userId);

    Task DeleteGroup(string ownerUserId);

    Task<PlogTogether> GetPlogTogether(string ownerUserId);

    Task LeaveGroup(string userId);
}

