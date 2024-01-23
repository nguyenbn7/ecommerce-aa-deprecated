using System.Linq.Expressions;
using Ecommerce.Module.Products.Model;
using Ecommerce.Shared.Database.Specification;

namespace Ecommerce.Module.Products.Specification;

public class ProductSpecification : CompositeSpecification<Product>
{
    private readonly int? productId;
    public ProductSpecification()
    {
    }

    public ProductSpecification(int id)
    {
        productId = id;
    }

    public override Expression<Func<Product, bool>> IsSatisfiedBy()
    {
        if (productId.HasValue)
            return p => p.Id == productId;
        return p => true;
    }
}