using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Exceptions;
using Ordering.Domain.ValueObjects.StrongTypeIDs;

namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler
        : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        private readonly IApplicationDbContext _db;

        public DeleteOrderCommandHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand request,
            CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(request.OrderId);
            var order = await _db.Orders
                .FindAsync([orderId], cancellationToken);

            if (order == null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync(cancellationToken);

            return new DeleteOrderResult(true);
        }
    }
}
