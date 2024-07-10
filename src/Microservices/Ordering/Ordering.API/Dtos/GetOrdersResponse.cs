using BuildingBlocks.Pagination;
using Ordering.Application.Dtos;

namespace Ordering.API.Dtos
{
    public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);
}
