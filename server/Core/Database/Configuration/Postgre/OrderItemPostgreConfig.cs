using Ecommerce.Module.Orders.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Core.Database.Configuration.Postgre;

public class OrderItemPostgreConfig : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(oi => oi.Price).HasColumnType("DECIMAL (18, 2)");
    }
}