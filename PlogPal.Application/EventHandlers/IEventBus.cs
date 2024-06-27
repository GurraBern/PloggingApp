namespace PlogPal.Application.Interfaces;

public interface IEventBus
{
    void Publish<TEvent>(TEvent @event);
}

public interface IEventHandler<TEvent>
{
    Task Handle(TEvent domainEvent);
}
