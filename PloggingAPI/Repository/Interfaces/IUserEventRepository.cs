using Plogging.Core.Models;

namespace PloggingAPI.Repository.Interfaces;

public interface IUserEventRepository
{
    Task CreateEvent(UserEvent userEvent);
    Task<IEnumerable<UserEvent>> GetEvents();
}