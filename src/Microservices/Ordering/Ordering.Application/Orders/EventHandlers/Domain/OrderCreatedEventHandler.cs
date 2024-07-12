using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Ordering.Application.Extensions;
using Ordering.Domain.DomainEvents;

namespace Ordering.Application.Orders.EventHandlers.Domain
{
    //this is consumer for the dispatched OrderCreatedEvent from Order aggregate
    public class OrderCreatedEventHandler
    : INotificationHandler<OrderCreatedEvent>
    //handles OrderCreatedEvent nofitications/events when they are dispatched
    //so this will be triggered once MediatR publish the OrderCreatedEvent - it is publish from DispatchDomainEventsInterceptor in my code
    //This means that when the OrderCreatedEvent is published, the handler within the same service will react to it
    {
        private readonly ILogger<OrderCreatedEventHandler> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IFeatureManager _featureMan;

        public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger,
            IPublishEndpoint publishEndpoint,
            IFeatureManager featureMan)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _featureMan = featureMan;
        }

        public async Task Handle(OrderCreatedEvent domainEvent,
            CancellationToken cancellationToken)
        {
            //Here the appliation should react to OrderCreatedEvent - some logic implemented
            //OrderCreatedEvent contains the event datails

            _logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

            //In Feature Flag
            //Map order entity to order dto
            //Once we have consumer of this event - we will set it to true
            if (await _featureMan.IsEnabledAsync("OrderFullfilment"))
            {
                var orderCreatedIntegrationEvent = domainEvent.Order.ToOrderDto();

                //Publish OrderCreatedIntegrationEvent to RabbitMQ
                await _publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
            }
        }
    }
}
