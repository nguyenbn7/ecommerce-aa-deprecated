using System.Linq.Expressions;
using Ecommerce.Module.Products.Model;
using Ecommerce.Shared;

namespace Ecommerce.Module.Products.Specification;

public class IncludeProductType : IIncludeSpecification<Product>
{
    public Expression<Func<Product, object>> IncludeProperty()
    {
        return p => p.ProductType;
    }
}