using Ordering.Domain.Abstractions;

namespace Ordering.Domain.Models
{
    //Entity
    public class Product : Entity<Guid>
    {
        public string Name { get; private set; } = default!;
        public decimal Price { get; private set; } = default!;
    }
}
