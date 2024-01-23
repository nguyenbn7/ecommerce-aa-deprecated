using Ecommerce.Module.Orders.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Core.Database.Configuration.Sqlite;

public class OrderItemSqliteConfig : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(p => p.Price).HasColumnType("decimal(18, 2)").HasConversion<double>();
    }
}