using PlogPal.Application.Interfaces;

namespace Infrastructure.EventBus;

public class EventBus(EventHandlerRegistry eventHandlerRegistry) : IEventBus 
{
    private readonly EventHandlerRegistry _eventHandlerRegistry = eventHandlerRegistry;

    public void Publish<TEvent>(TEvent @event)
    {
        var handlers = _eventHandlerRegistry.GetHandlers<TEvent>();

        foreach (var handler in handlers)
        {
            handler.Handle(@event);
        }
    }

    //private readonly Dictionary<Type, List<IEventHandler>> _subscriptions = [];

    //public void Register<TEvent>(IEventHandler handler)
    //{
    //    var eventType = typeof(TEvent);
    //    if (!_subscriptions.TryGetValue(eventType, out var eventHandlers))
    //        _subscriptions[eventType] = new List<IEventHandler>();

    //    if (eventHandlers != null && eventHandlers.Contains(handler, new EventHandlerComparer()))
    //        return;

    //    _subscriptions[eventType].Add(handler);
    //}

    //public void Publish<TEvent>(TEvent @event) where TEvent : IDomainEvent
    //{
    //    if (_subscriptions.TryGetValue(typeof(TEvent), out var handlers))
    //    {
    //        foreach (var handler in handlers)
    //        {
    //            handler.Handle(@event);
    //        }
    //    }
    //}
}


