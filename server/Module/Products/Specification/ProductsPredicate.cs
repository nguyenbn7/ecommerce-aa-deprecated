using System.Linq.Expressions;
using Ecommerce.Module.Products.Model;
using Ecommerce.Shared;

namespace Ecommerce.Module.Products.Specification;

public class ProductsPredicate : IPredicateSpecification<Product>
{
    private readonly int? brandId;
    private readonly int? typeId;
    private readonly string? searchTerm;

    public ProductsPredicate(int? brandId, int? typeId, string? searchTerm)
    {
        this.brandId = brandId;
        this.typeId = typeId;
        this.searchTerm = searchTerm?.ToLower();
    }

    public Expression<Func<Product, bool>>? ToPredicate(PredicateBuilder<Product> builder)
    {
        var criteria = builder.Construct();
        if (brandId != null)
        {
            criteria.And(p => p.ProductBrandId == brandId);
        }

        if (typeId != null)
        {
            criteria.And(p => p.ProductTypeId == typeId);
        }

        if (searchTerm != null)
        {
            criteria.And(p => p.Name.ToLower().Contains(searchTerm));
        }

        return criteria.ToPredicate();
    }
}