using Ecommerce.Module.Orders.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Core.Database.Configuration;

public class OrderSharedConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(o => o.ShipToAddress,
                        a => { a.WithOwner(); });

        builder.Property(o => o.OrderStatus)
            .HasConversion(s => s.ToString(),
                           x => (Status)Enum.Parse(typeof(Status), x));

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}