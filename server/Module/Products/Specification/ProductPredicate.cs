using System.Linq.Expressions;
using Ecommerce.Module.Products.Model;
using Ecommerce.Shared;

namespace Ecommerce.Module.Products.Specification;

public class ProductPredicate : IPredicateSpecification<Product>
{
    private readonly int id;

    public ProductPredicate(int id)
    {
        this.id = id;
    }

    public Expression<Func<Product, bool>>? ToPredicate(PredicateBuilder<Product> builder)
    {
        return builder.Construct(p => p.Id == id).ToPredicate();
    }
}