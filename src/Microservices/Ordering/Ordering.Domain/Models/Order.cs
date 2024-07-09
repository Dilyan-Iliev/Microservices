using Ordering.Domain.Abstractions;
using Ordering.Domain.DomainEvents;
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
        public static Order Create(OrderId id, CustomerId customerId, OrderName orderName,
            Address shippingAddress, Address billingAddress, Payment payment)
        {
            var order = new Order
            {
                Id = id,
                CustomerId = customerId,
                OrderName = orderName,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                Payment = payment,
                Status = OrderStatus.Pending,
            };

            order.AddDomainEvent(new OrderCreatedEvent(order));

            return order;
        }

        public void Update(OrderName orderName, Address shippingAddress, Address billingAddress,
            Payment payment, OrderStatus status)
        {
            OrderName = orderName;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Payment = payment;
            Status = status;

            this.AddDomainEvent(new OrderUpdatedEvent(this));
        }

        public void Add(ProductId productId, int quantity, decimal price)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var orderItem = new OrderItem(this.Id, productId, quantity, price);
            _orderItems.Add(orderItem);
        }

        public void Remove(ProductId productId)
        {
            var item = _orderItems.FirstOrDefault(item => item.ProductId == productId);

            if (item != null)
            {
                _orderItems.Remove(item);
            }
        }
    }
}
