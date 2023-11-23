using System.Linq.Expressions;
using Ecommerce.Share;

namespace Ecommerce.API.Product.Specification;

public class ProductsSpecification : ISpecification<Model.Product>
{
    private readonly int? brandId;
    private readonly int? typeId;
    private readonly string? searchTerm;

    public ProductsSpecification(int? brandId, int? typeId, string? searchTerm)
    {
        this.brandId = brandId;
        this.typeId = typeId;
        this.searchTerm = searchTerm?.ToLower();
    }

    public Expression<Func<Model.Product, bool>>? ToPredicate(CriteriaBuilder<Model.Product> builder)
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

public class ProductSpecification : ISpecification<Model.Product>
{
    private readonly int id;

    public ProductSpecification(int id)
    {
        this.id = id;
    }

    public Expression<Func<Model.Product, bool>>? ToPredicate(CriteriaBuilder<Model.Product> builder)
    {
        return builder.Construct(p => p.Id == id).ToPredicate();
    }
}