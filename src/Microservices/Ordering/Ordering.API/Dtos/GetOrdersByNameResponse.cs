using Ordering.Application.Dtos;

namespace Ordering.API.Dtos
{
    public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);
}
