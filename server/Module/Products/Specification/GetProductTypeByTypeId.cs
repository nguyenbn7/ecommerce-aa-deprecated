using System.Linq.Expressions;
using Ecommerce.Module.Products.Model;
using Ecommerce.Shared.Database.Criteria;

namespace Ecommerce.Module.Products.Specification;

public class GetProductTypeByTypeId : CompositeSpecification<Product>
{
    private readonly int typeId;
    
    public GetProductTypeByTypeId(int id)
    {
        typeId = id;
    }

    public override Expression<Func<Product, bool>> IsSatisfiedBy()
    {
        return p => p.ProductTypeId == typeId;
    }
}