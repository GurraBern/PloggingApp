using Plogging.Core.Models;

namespace PloggingAPI.Repository.Interfaces;

public interface IPlogTogetherRepository
{
    //Task<PlogTogether> FindOrCreateGroup(string ownerUserId);

    Task AddUserToGroup(string ownerUserId, string addUserId);

    Task DeleteGroup(string ownerUserId);
}

