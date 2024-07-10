using Ordering.Application.Dtos;
using Ordering.Domain.Models;

namespace Ordering.Application.Extensions
{
    public static class OrderExtensions
    {
        public static IEnumerable<OrderDto> ToOrderDtoCollection(this IEnumerable<Order>? orders)
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
