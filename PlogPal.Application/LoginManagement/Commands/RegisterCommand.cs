using PlogPal.Application.Common.Interfaces;
using PlogPal.Application.Errors;
using PlogPal.Application.Interfaces;
using PlogPal.Domain.Events;

namespace PlogPal.Application.LoginManagement.Commands;

public interface IRegisterCommand
{
    Task<Result> RegisterUser(string email, string password, string displayName);
}

public class RegisterCommand(IAuthenticationService authenticationService, IEventBus eventBus) : IRegisterCommand
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IEventBus _eventBus = eventBus;

    public async Task<Result> RegisterUser(string email, string password, string displayName)
    {
        try
        {
            string userId = await _authenticationService.CreateUser(email, password, displayName);
            
            _eventBus.Publish(new SignUpEvent()
            {
                UserId = userId,
                Email = email,
                Password = password,
                DisplayName = displayName
            });

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(LoginErrors.AccountNotCreated);
        }
    }
}

