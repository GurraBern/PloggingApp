using Plogging.Core.Models;

namespace PloggingApp.Data.Services.Interfaces;

public interface IUserEventService
{
    Task CreateEvent(UserEvent userEvent);
}