using Ordering.Domain.Abstractions;

namespace Ordering.Domain.Models
{
    //This is entity
    public class OrderItem : Entity<Guid>
    {
        internal OrderItem(Guid orderId, Guid productId, int quantity, decimal price)
        {
            this.OrderId = orderId;
            this.ProductId = productId;
            this.Quantity = quantity;
            this.Price = price;
        }

        public Guid OrderId { get; private set; } = default!;
        public Guid ProductId { get; private set; } = default!;
        public int Quantity { get; private set; } = default!;
        public decimal Price { get; private set; } = default!;
    }
}
