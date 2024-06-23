using PlogPal.Domain.Events;

namespace PlogPal.Domain.Interfaces;

public interface IEventBus
{
    void Register<TEvent>(IEventHandler handler);

    void Publish<TEvent>(IDomainEvent @event);
}
