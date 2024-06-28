using PlogPal.Application.Common.Interfaces;
using PlogPal.Application.Interfaces;
using PlogPal.Domain.Events;

namespace PlogPal.Application.LoginManagement.Commands;

public interface ILoginCommand
{
    Task<bool> LoginUser(string email, string password);
}

public class LoginCommand(IAuthenticationService authenticationService, IEventBus eventBus) : ILoginCommand
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IEventBus _eventBus = eventBus;

    public async Task<bool> LoginUser(string email, string password)
    {
        var result = await _authenticationService.LoginUser(email, password);

        if (result)
        {
            _eventBus.Publish(new SignInEvent());
        }

        return result;
    }
}
