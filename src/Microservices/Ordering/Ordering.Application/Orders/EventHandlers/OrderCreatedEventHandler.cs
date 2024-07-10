using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.DomainEvents;

namespace Ordering.Application.Orders.EventHandlers
{
    //this is consumer for the dispatched OrderCreatedEvent from Order aggregate
    public class OrderCreatedEventHandler
    : INotificationHandler<OrderCreatedEvent>
    //handles OrderCreatedEvent nofitications/events when they are dispatched
    //so this will be triggered once MediatR publish the OrderCreatedEvent - it is publish from DispatchDomainEventsInterceptor in my code
    {
        private readonly ILogger<OrderCreatedEventHandler> _logger;

        public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderCreatedEvent notification,
            CancellationToken cancellationToken)
        {
            //Here the appliation should react to OrderCreatedEvent - some logic implemented
            //OrderCreatedEvent contains the event datails

            _logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
