using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Extensions;
using Ordering.Application.Orders.Queries.GetOrderByName;

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
                .OrderBy(o => o.OrderName.Value)
                .ToListAsync(cancellationToken);

            var result = orders.ToOrderDtoCollection();

            return new GetOrdersByNameResult(result);
        }
    }
}
