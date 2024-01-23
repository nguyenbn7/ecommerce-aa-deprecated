using System.Linq.Expressions;
using Ecommerce.Module.Products.Model;
using Ecommerce.Shared.Database.Specification;

namespace Ecommerce.Module.Products.Specification;

public class GetProductBySearchTerm : CompositeSpecification<Product>
{
    private readonly string searchTerm;

    public GetProductBySearchTerm(string search)
    {
        searchTerm = search;
    }
    
    public override Expression<Func<Product, bool>> IsSatisfiedBy()
    {
        return p => p.Name.ToLower().Contains(searchTerm);
    }
}