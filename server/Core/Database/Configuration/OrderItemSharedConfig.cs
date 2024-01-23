using Ecommerce.Module.Orders.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Core.Database.Configuration;

public class OrderItemSharedConfig : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.OwnsOne(i => i.ItemOrdered, io => { io.WithOwner(); });
    }
}