using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Queries.GetOrderByName;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders.Queries.GetOrdersByName
{
    public class GetOrdersByNameQueryHandler
        : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        private readonly IApplicationDbContext _db;

        public GetOrdersByNameQueryHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery request,
            CancellationToken cancellationToken)
        {
            var orders = await _db.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .Where(o => o.OrderName.Value.Contains(request.Name))
                .OrderBy(o => o.OrderName)
                .ToListAsync(cancellationToken);

            var result = ProjectToOrderDto(orders);

            return new GetOrdersByNameResult(result);
        }

        private IEnumerable<OrderDto> ProjectToOrderDto(List<Order>? orders)
        {
            if (orders?.Any() ?? false)
            {
                List<OrderDto> result = new();
                foreach (var order in orders)
                {
                    var shippingAddresDto = new AddressDto(
                        FirstName: order.ShippingAddress.FirstName,
                        LastName: order.ShippingAddress.LastName,
                        EmailAddress: order.ShippingAddress.EmailAddress,
                        AddressLine: order.ShippingAddress.AddressLine,
                        Country: order.ShippingAddress.Country,
                        State: order.ShippingAddress.State,
                        ZipCode: order.ShippingAddress.ZipCode);

                    var billingAddressDto = new AddressDto(
                        FirstName: order.BillingAddress.FirstName,
                        LastName: order.BillingAddress.LastName,
                        EmailAddress: order.BillingAddress.EmailAddress,
                        AddressLine: order.BillingAddress.AddressLine,
                        Country: order.BillingAddress.Country,
                        State: order.BillingAddress.State,
                        ZipCode: order.BillingAddress.ZipCode);

                    var paymentDto = new PaymentDto(
                        CardName: order.Payment.CardName,
                        CardNumber: order.Payment.CardNumber,
                        Expiration: order.Payment.Expiration,
                        Cvv: order.Payment.CVV,
                        PaymentMethod: order.Payment.PaymentMethod);

                    var orderItemsDto = order.OrderItems
                        .Select(item => new OrderItemDto(
                            OrderId: item.OrderId.Value,
                            ProductId: item.ProductId.Value,
                            Quantity: item.Quantity,
                            Price: item.Price))
                        .ToList();

                    var orderDto = new OrderDto(
                        Id: order.Id.Value,
                        CustomerId: order.CustomerId.Value,
                        OrderName: order.OrderName.Value,
                        ShippingAddress: shippingAddresDto,
                        BillingAddress: billingAddressDto,
                        Payment: paymentDto,
                        Status: order.Status,
                        OrderItems: orderItemsDto);

                    result.Add(orderDto);
                    return result;
                }
            }

            return Array.Empty<OrderDto>();
        }
    }
}
