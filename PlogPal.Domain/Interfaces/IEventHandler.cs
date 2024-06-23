using PlogPal.Domain.Events;

namespace PlogPal.Domain.Interfaces;

public interface IEventHandler
{
    Task Handle(IDomainEvent domainEvent);
}
