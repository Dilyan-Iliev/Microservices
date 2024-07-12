﻿using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Application.Orders.EventHandlers.Integration
{
    public class BasketCheckoutEventHandler
        : IConsumer<BasketCheckoutEvent>
    //here we do not use INotificationHandler<T> because now we have cross-services communication
    //This means that when the BasketCheckoutEvent is published,
    //it can be consumed by multiple services listening for this even
    {
        private readonly ISender _sender;
        private readonly ILogger<BasketCheckoutEventHandler> _logger;

        public BasketCheckoutEventHandler(ISender sender,
            ILogger<BasketCheckoutEventHandler> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            //ConsumeContext will provide access to event data 
            //Once we recieve the message - we will create new order => we will send new CreateOrderCommand

            _logger.LogInformation("Integration Event handled: {IntegrationEvent}",
                context.Message.GetType().Name);

            //Create order operation:
            var command = MapToCreateOrderCommand(context.Message);
            //This will hit CreateOrderCommandHandler
            await _sender.Send(command);
        }

        private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
        {
            // Create full order with incoming event data
            var addressDto = new AddressDto(message.FirstName, message.LastName, message.EmailAddress, message.AddressLine, message.Country, message.State, message.ZipCode);
            var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.CVV, message.PaymentMethod);
            var orderId = Guid.NewGuid();

            var orderDto = new OrderDto(
                Id: orderId,
                CustomerId: message.CustomerId,
                OrderName: message.UserName,
                ShippingAddress: addressDto,
                BillingAddress: addressDto,
                Payment: paymentDto,
                Status: Ordering.Domain.Enums.OrderStatus.Pending,
                OrderItems:
                [
                    new OrderItemDto(orderId, new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"), 2, 500),
                    new OrderItemDto(orderId, new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"), 1, 400)
                ]);

            return new CreateOrderCommand(orderDto);
        }
    }
}
