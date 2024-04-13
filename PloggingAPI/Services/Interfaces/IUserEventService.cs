using Plogging.Core.Models;

namespace PloggingAPI.Services.Interfaces;

public interface IUserEventService
{
    Task CreateEvent(UserEvent userEvent);
    Task<IEnumerable<UserEvent>> GetEvents();
}