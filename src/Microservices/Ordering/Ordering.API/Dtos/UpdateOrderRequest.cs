using Ordering.Application.Dtos;

namespace Ordering.API.Dtos
{
    public record UpdateOrderRequest(OrderDto Order);
    public record UpdateOrderResponse(bool IsSuccess);
}
