using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects.StrongTypeIDs;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration
        : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasConversion( //converts from CustomerId type into Guid to store it in DB
                customerId => customerId.Value, //This lambda expression converts the CustomerId object to its underlying Guid value before storing it in the database
                dbId => CustomerId.Of(dbId)); //This lambda expression converts the Guid value from the database back to a CustomerId objec

            builder.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(255);

            builder.HasIndex(c => c.Email).IsUnique();
        }
    }
}
