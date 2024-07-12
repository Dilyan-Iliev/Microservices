using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.DomainEvents;

namespace Ordering.Application.Orders.EventHandlers.Domain
{
    //this is consumer for the dispatched OrderUpdatedEvent from Order aggregate
    public class OrderUpdatedEventHandler
        : INotificationHandler<OrderUpdatedEvent>
    {
        private readonly ILogger<OrderUpdatedEventHandler> _logger;

        public OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderUpdatedEvent notification,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
