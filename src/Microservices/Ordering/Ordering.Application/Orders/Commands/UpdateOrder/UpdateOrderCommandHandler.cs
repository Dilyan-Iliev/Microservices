using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Exceptions;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using Ordering.Domain.ValueObjects.StrongTypeIDs;

namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler
        : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        private readonly IApplicationDbContext _db;

        public UpdateOrderCommandHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand request,
            CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(request.Order.Id);

            var order = await _db.Orders
                .FindAsync([orderId], cancellationToken);

            if (order == null)
            {
                throw new OrderNotFoundException(request.Order.Id);
            }

            UpdateOrder(order, request.Order);
            _db.Orders.Update(order);
            await _db.SaveChangesAsync(cancellationToken);

            return new UpdateOrderResult(true);
        }

        private void UpdateOrder(Order origin, OrderDto incoming)
        {
            //value objects
            var updatedShippingAddress = Address.Of(incoming.ShippingAddress.FirstName, incoming.ShippingAddress.LastName, incoming.ShippingAddress.EmailAddress, incoming.ShippingAddress.AddressLine, incoming.ShippingAddress.Country, incoming.ShippingAddress.State, incoming.ShippingAddress.ZipCode);
            var updatedBillingAddress = Address.Of(incoming.BillingAddress.FirstName, incoming.BillingAddress.LastName, incoming.BillingAddress.EmailAddress, incoming.BillingAddress.AddressLine, incoming.BillingAddress.Country, incoming.BillingAddress.State, incoming.BillingAddress.ZipCode);
            var updatedPayment = Payment.Of(incoming.Payment.CardName, incoming.Payment.CardNumber, incoming.Payment.Expiration, incoming.Payment.Cvv, incoming.Payment.PaymentMethod);
            var updatedOrderName = OrderName.Of(incoming.OrderName);

            origin.Update(
                orderName: updatedOrderName, 
                shippingAddress: updatedShippingAddress,
                billingAddress: updatedBillingAddress,
                payment: updatedPayment,
                status: incoming.Status);
        }
    }
}
