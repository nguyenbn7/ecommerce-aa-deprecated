using System.Linq.Expressions;
using Ecommerce.Module.Products.Model;
using Ecommerce.Shared.Database.Criteria;

namespace Ecommerce.Module.Products.Specification;

public class GetProductBrandByBrandId : CompositeSpecification<Product>
{
    private readonly int brandId;
    public GetProductBrandByBrandId(int id)
    {
        brandId = id;
    }

    public override Expression<Func<Product, bool>> IsSatisfiedBy()
    {
        return p => p.ProductBrandId == brandId;
    }
}