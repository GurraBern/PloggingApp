using PlogPal.Application.Common.Interfaces;
using PlogPal.Application.Errors;
using PlogPal.Application.Interfaces;
using PlogPal.Domain.Events;

namespace PlogPal.Application.LoginManagement.Commands;

public interface ILoginCommand
{
    Task<Result> LoginUser(string email, string password);
}

public class LoginCommand(IUserContext userContext, IEventBus eventBus) : ILoginCommand
{
    private readonly IUserContext _userContext = userContext;
    private readonly IEventBus _eventBus = eventBus;

    public async Task<Result> LoginUser(string email, string password)
    {
        await _userContext.Login(email, password);

        if (_userContext.IsAuthenticated)
        {
            _eventBus.Publish(new SignInEvent());
            return Result.Success();
        }

        return Result.Failure(LoginErrors.InvalidCredentials);
    }
}
