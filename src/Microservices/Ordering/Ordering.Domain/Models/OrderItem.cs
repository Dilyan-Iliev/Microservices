using Ordering.Domain.Abstractions;
using Ordering.Domain.ValueObjects.StrongTypeIDs;

namespace Ordering.Domain.Models
{
    //This is entity
    public class OrderItem : Entity<OrderItemId>
    {
        internal OrderItem(OrderId orderId, ProductId productId, int quantity, decimal price)
        {
            this.Id = OrderItemId.Of(Guid.NewGuid());
            this.OrderId = orderId;
            this.ProductId = productId;
            this.Quantity = quantity;
            this.Price = price;
        }

        public OrderId OrderId { get; private set; } = default!;
        public ProductId ProductId { get; private set; } = default!;
        public int Quantity { get; private set; } = default!;
        public decimal Price { get; private set; } = default!;
    }
}
