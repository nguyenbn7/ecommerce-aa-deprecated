using AutoMapper;
using Ecommerce.Module.Products;

namespace Ecommerce.Core;

public class ProductURLResolver : IValueResolver<Product, ProductDTO, string?>
{
    private readonly IConfiguration _configuration;

    public ProductURLResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string? Resolve(Product source, ProductDTO destination, string? destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.PictureUrl))
        {
            return _configuration["ApiUrl"] + source.PictureUrl;
        }
        return null;
    }
}