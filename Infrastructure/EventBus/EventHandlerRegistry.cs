using PlogPal.Application.Interfaces;

namespace Infrastructure.EventBus;

public class EventHandlerRegistry
{
    private readonly Dictionary<Type, object> handlers = [];

    public void AddHandler<TEvent>(IEventHandler<TEvent> handler)
    {
        if (!handlers.TryGetValue(typeof(TEvent), out object? value))
        {
            value = new List<IEventHandler<TEvent>>();
            handlers[typeof(TEvent)] = value;
        }

        ((List<IEventHandler<TEvent>>)value).Add(handler);
    }

    public bool HasHandler<T>() => handlers.ContainsKey(typeof(T));

    public IReadOnlyCollection<IEventHandler<TEvent>> GetHandlers<TEvent>()
        => handlers.TryGetValue(typeof(TEvent), out var list) ? (List<IEventHandler<TEvent>>)list : Array.Empty<IEventHandler<TEvent>>();
}
