using Ecommerce.Module.Orders.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Core.Database.Configuration.Postgre;

public class DeliveryMethodPostgreConfig : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.Property(dm => dm.Price).HasColumnType("decimal(18, 2)");
    }
}