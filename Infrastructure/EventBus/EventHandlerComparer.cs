using PlogPal.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.EventBus;

internal class EventHandlerComparer : IEqualityComparer<IEventHandler>
{
    public bool Equals(IEventHandler? x, IEventHandler? y)
    {
        if (x is null || y is null) return false;

        return x.GetType().Equals(y.GetType());
    }

    public int GetHashCode([DisallowNull] IEventHandler obj)
    {
        throw new NotImplementedException();
    }
}
