using PlogPal.Application.Common.Interfaces;
using PlogPal.Application.Interfaces;
using PlogPal.Domain.Events;

namespace PlogPal.Application.LoginManagement.Commands;

public interface IRegisterCommand
{
    Task<bool> RegisterUser(string email, string password, string displayName);
}

public class RegisterCommand(IAuthenticationService authenticationService, IEventBus eventBus) : IRegisterCommand
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IEventBus _eventBus = eventBus;

    public async Task<bool> RegisterUser(string email, string password, string displayName)
    {
        var userId = await _authenticationService.CreateUser(email, password, displayName);
        if (!string.IsNullOrEmpty(userId))
        {
            _eventBus.Publish(new SignUpEvent()
            {
                UserId = userId,
                Email = email,
                Password = password,
                DisplayName = displayName
            });

            return true;
        }
            
        return false;
    }
}

