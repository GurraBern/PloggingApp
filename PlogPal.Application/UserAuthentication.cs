using PlogPal.Application.Common.Interfaces;
using PlogPal.Application.Interfaces;
using PlogPal.Domain.Events;

namespace PlogPal.Application;

public class UserAuthentication : IUserAuthentication
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IEventBus _eventBus;

    public UserAuthentication(IAuthenticationService authenticationService, IEventBus eventBus)
    {
        _authenticationService = authenticationService;
        _eventBus = eventBus;
    }

    public async Task CreateUser(string email, string password, string displayName)
    {
        await _authenticationService.CreateUser(email, password, displayName);
        _eventBus.Publish(new SignUpEvent());
    }

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
