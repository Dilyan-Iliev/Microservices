using Ordering.Domain.Abstractions;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;
using Ordering.Domain.ValueObjects.StrongTypeIDs;

namespace Ordering.Domain.Models
{
    //This is OrderAggregate - order aggregate root including all related entities and value objects
    //Aggregate root is an entity that acts as entrypoint for the aggregate
    public class Order : Aggregate<OrderId>
    {
        //List of OrderItem entities
        private readonly List<OrderItem> _orderItems = new();

        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public CustomerId CustomerId { get; private set; } = default!;

        public OrderName OrderName { get; private set; } = default!;

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

        //Richt-domain model bussiness logic
        public void AddOrderItem(OrderItem orderItem)
        {
            _orderItems.Add(orderItem);
        }

        public void RemoveOrderItem(OrderItem orderItem)
        {
            var item = _orderItems.FirstOrDefault(item => item.Id == orderItem.Id);

            if (item != null)
            {
                _orderItems.Remove(orderItem);
            }
        }
    }
}
