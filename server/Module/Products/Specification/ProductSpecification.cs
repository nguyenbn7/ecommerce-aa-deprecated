using System.Linq.Expressions;
using Ecommerce.Module.Products.Model;
using Ecommerce.Shared.Database.Criteria;

namespace Ecommerce.Module.Products.Specification;

public class ProductSpecification : CompositeSpecification<Product>
{
    public override Expression<Func<Product, bool>> IsSatisfiedBy()
    {
        return p => true;
    }
}