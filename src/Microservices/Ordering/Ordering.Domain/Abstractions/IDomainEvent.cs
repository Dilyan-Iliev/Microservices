using MediatR;

namespace Ordering.Domain.Abstractions
{
    public interface IDomainEvent
        : INotification
    { //INotification from MediatR allows domain events to be dispatched through mediator handlers

        Guid EventId => Guid.NewGuid();
        public DateTime OccuredOn => DateTime.Now;
        public string EventTpe => GetType().AssemblyQualifiedName;
    }
}
