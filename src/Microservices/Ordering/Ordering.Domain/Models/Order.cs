using Ordering.Domain.Abstractions;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Models
{
    //This is OrderAggregate - order aggregate root including all related entities and value objects
    //Aggregate root is an entity that acts as entrypoint for the aggregate
    public class Order : Aggregate<Guid>
    {
        //List of OrderItem entities
        private readonly List<OrderItem> _orderItems = new();

        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public Guid CustomerId { get; private set; } = default!;

        public string OrderName { get; private set; } = default!;

        //Value object
        public Address ShippingAddress { get; private set; } = default!;

        //Value object
        public Address BillingAddress { get; private set; } = default!;

        //Value object
        public Payment Payment { get; private set; } = default!;

        public OrderStatus Status { get; private set; } = OrderStatus.Pending;

        public decimal TotalPrice
        {
            get => OrderItems.Sum(x => x.Price * x.Quantity);
            private set { }
        }
    }
}
