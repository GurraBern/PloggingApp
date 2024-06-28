namespace PlogPal.Domain.Events;

public class SignUpEvent : IDomainEvent
{
    public required string UserId { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string DisplayName { get; init; }
}

