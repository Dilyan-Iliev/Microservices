using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrders
{
    public class GetOrdersQueryHandler
        : IQueryHandler<GetOrdersQuery, GetOrdersResult>
    {
        private readonly IApplicationDbContext _db;

        public GetOrdersQueryHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<GetOrdersResult> Handle(GetOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var pageIndex = request.PaginationRequest.PageIndx;
            var pageSize = request.PaginationRequest.PageSize;
            var totalOrdersCount = await _db.Orders
                .LongCountAsync(cancellationToken);

            var orders = await _db.Orders
                .Include(o => o.OrderItems)
                .OrderBy(o => o.OrderName.Value)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var result = new PaginatedResult<OrderDto>(
                pageIndex,
                pageSize,
                totalOrdersCount,
                orders.ToOrderDtoCollection());

            return new GetOrdersResult(result);
        }
    }
}
