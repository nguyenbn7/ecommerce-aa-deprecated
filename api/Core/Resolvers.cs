using AutoMapper;
using Ecommerce.Routes.Products;

namespace Ecommerce.Core.Resolvers;

public class ProductURLResolver : IValueResolver<Product, ProductDTO, string?>
{
    private readonly IConfiguration config;

    public ProductURLResolver(IConfiguration config)
    {
        this.config = config;
    }

    public string? Resolve(Product source, ProductDTO destination, string? destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.PictureUrl))
        {
            return config["ApiUrl"] + source.PictureUrl;
        }
        return null;
    }
}