using Ordering.Application.Dtos;

namespace Ordering.API.Dtos
{
    public record CreateOrderRequest(OrderDto Order);
    public record CreateOrderReponse(Guid Id);
}
