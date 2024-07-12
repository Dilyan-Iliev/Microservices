using Ordering.Domain.Abstractions;
using Ordering.Domain.Models;

namespace Ordering.Domain.DomainEvents
{
    public record OrderCreatedEvent(Order Order) : IDomainEvent;
}
