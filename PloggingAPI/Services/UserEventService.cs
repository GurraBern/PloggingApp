using Plogging.Core.Models;
using PloggingAPI.Repository.Interfaces;
using PloggingAPI.Services.Interfaces;

namespace PloggingAPI.Services;

public class UserEventService : IUserEventService
{
    private readonly IUserEventRepository _userEventRepository;

    public UserEventService(IUserEventRepository userEventRepository)
    {
        _userEventRepository = userEventRepository;
    }

    public async Task CreateEvent(UserEvent userEvent)
    {
        await _userEventRepository.CreateEvent(userEvent);
    }

    public async Task<IEnumerable<UserEvent>> GetEvents()
    {
        return await _userEventRepository.GetEvents();
    }
}
