using PlogPal.Application.Common.Interfaces;
using PlogPal.Application.Interfaces;
using PlogPal.Domain.Events;

namespace PlogPal.Application.EventHandlers;

public class SignInHandler(IUserContext userContext, IStreakManager streakManager) : IEventHandler<SignInEvent>
{
    private readonly IUserContext _userContext = userContext;
    private readonly IStreakManager _streakManager = streakManager;

    public async Task Handle(SignInEvent domainEvent)
    {
        _userContext.User.Streak = await _streakManager.GetStreak(_userContext.UserId);
    }
}