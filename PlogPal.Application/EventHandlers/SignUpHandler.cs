using PlogPal.Application.Common.Interfaces;
using PlogPal.Application.Interfaces;
using PlogPal.Domain.Events;

namespace PlogPal.Application.EventHandlers;

public class SignUpHandler : IEventHandler<SignUpEvent>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IStreakService _streakService;

    public SignUpHandler(IAuthenticationService authenticationService, IStreakService streakService)
    {
        _authenticationService = authenticationService;
        _streakService = streakService;
    }

    public async Task Handle(SignUpEvent domainEvent)
    {
        //TODO remove create user and do it inside the patch instead
        await _streakService.CreateUser(domainEvent.UserId);

        //TODO do we need user info?
        //await _userInfoService.CreateUser(domainEvent, domainEvent.DisplayName);

        await _authenticationService.LoginUser(domainEvent.Email, domainEvent.Password);
    }
}
