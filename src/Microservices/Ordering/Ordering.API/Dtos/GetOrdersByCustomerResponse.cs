using Ordering.Application.Dtos;

namespace Ordering.API.Dtos
{
    public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);
}
