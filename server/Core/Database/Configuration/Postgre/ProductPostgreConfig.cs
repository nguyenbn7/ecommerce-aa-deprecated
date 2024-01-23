using Ecommerce.Module.Products.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Core.Database.Configuration.Postgre;

public class ProductPostgreConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Price).HasColumnType("decimal(18, 2)");
    }
}