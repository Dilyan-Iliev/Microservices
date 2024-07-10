using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Extensions;
using Ordering.Domain.ValueObjects.StrongTypeIDs;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    public class GetOrdersByCustomerQueryHandler
        : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
    {
        private readonly IApplicationDbContext _db;

        public GetOrdersByCustomerQueryHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery request,
            CancellationToken cancellationToken)
        {
            var orders = await _db.Orders
                        .Include(o => o.OrderItems)
                        .AsNoTracking()
                        .Where(o => o.CustomerId == CustomerId.Of(request.CustomerId))
                        .OrderBy(o => o.OrderName.Value)
                        .ToListAsync(cancellationToken);

            return new GetOrdersByCustomerResult(orders.ToOrderDtoCollection());
        }
    }
}
