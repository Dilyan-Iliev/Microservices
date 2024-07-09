using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects.StrongTypeIDs;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration
        : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(item => item.Id);
            builder.Property(item => item.Id)
                .HasConversion(
                orderItemId => orderItemId.Value,
                dbId => OrderItemId.Of(dbId));

            builder.HasOne<Product>() //each orderItem relates to one product
                .WithMany() //one product has many OrderItems
                .HasForeignKey(item => item.ProductId);

            builder.Property(item => item.Quantity).IsRequired();
            builder.Property(item => item.Price).IsRequired();
        }
    }
}
