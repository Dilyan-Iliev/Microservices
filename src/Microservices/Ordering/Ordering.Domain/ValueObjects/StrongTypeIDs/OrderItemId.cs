using Ordering.Domain.Exceptions;

namespace Ordering.Domain.ValueObjects.StrongTypeIDs
{
    public record OrderItemId
    {
        private OrderItemId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static OrderItemId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (value == Guid.Empty)
            {
                throw new DomainException("OrderItemId cannot be empty");
            }

            return new OrderItemId(value);
        }
    }
}
