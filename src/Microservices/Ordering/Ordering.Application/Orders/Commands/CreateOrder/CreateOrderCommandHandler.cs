using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects.StrongTypeIDs;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler
        : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        private readonly IApplicationDbContext _db;

        public CreateOrderCommandHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<CreateOrderResult> Handle(CreateOrderCommand request,
            CancellationToken cancellationToken)
        {
            var order = CreateNewOrder(request.Order);

            await _db.Orders.AddAsync(order, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return new CreateOrderResult(order.Id.Value);
        }

        private Order CreateNewOrder(OrderDto orderDto)
        {
            //create value objects
            var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
            var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
            var payment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);
            var orderId = OrderId.Of(Guid.NewGuid());
            var customerId = CustomerId.Of(orderDto.CustomerId);
            var orderName = OrderName.Of(orderDto.OrderName);

            //create order
            var newOrder = Order.Create(
                    id: orderId,
                    customerId: customerId,
                    orderName: orderName,
                    shippingAddress: shippingAddress,
                    billingAddress: billingAddress,
                    payment: payment
                    );

            //add order items
            foreach (var orderItemDto in orderDto.OrderItems)
            {
                newOrder
                    .Add(ProductId.Of(orderItemDto.ProductId), orderItemDto.Quantity, orderItemDto.Price);
            }

            return newOrder;
        }
    }
}
