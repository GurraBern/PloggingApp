using PlogPal.Domain.Models;

namespace PloggingAPI.Features.Plogtogether;

public interface IPlogTogetherRepository
{
    Task AddUserToGroup(string ownerUserId, string userId);

    Task DeleteGroup(string ownerUserId);

    Task<PlogTogether> GetPlogTogether(string ownerUserId);

    Task LeaveGroup(string userId);
}

