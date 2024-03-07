namespace PloggingApp.Data.Services;

public interface IPlogTogetherService
{
    Task AddUserToGroup(string ownerUserId, string userId);

    Task DeleteGroup(string ownerUserId);
}