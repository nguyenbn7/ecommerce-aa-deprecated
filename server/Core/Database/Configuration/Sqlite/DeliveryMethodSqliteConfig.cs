using Ecommerce.Module.Orders.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Core.Database.Configuration;

public class DeliveryMethodSqliteConfig : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.Property(p => p.Price).HasColumnType("decimal(18, 2)").HasConversion<double>();
    }
}