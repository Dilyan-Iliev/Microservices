using Ordering.Domain.Abstractions;
using Ordering.Domain.Models;

namespace Ordering.Domain.DomainEvents
{
    public record OrderUpdatedEvent(Order Order) : IDomainEvent;
}
