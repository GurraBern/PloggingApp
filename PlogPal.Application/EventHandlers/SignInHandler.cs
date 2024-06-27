using PlogPal.Application.Common.Interfaces;
using PlogPal.Application.Interfaces;
using PlogPal.Domain.Events;

namespace PlogPal.Application.EventHandlers;

public class SignInHandler(IStreakService streakService) : IEventHandler<SignInEvent>
{
    private readonly IStreakService _streakService = streakService;

    public async Task Handle(SignInEvent domainEvent)
    {
        await _streakService.ResetStreak();
    }
}