using AutoMapper;
using Ecommerce.Module.Products;
using Ecommerce.Module.Products.DTO;
using Ecommerce.Module.Products.Model;

namespace Ecommerce.Core;

public class ProductURLResolver : IValueResolver<Product, ProductReponse, string?>
{
    private readonly IConfiguration _configuration;

    public ProductURLResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string? Resolve(Product source, ProductReponse destination, string? destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.PictureUrl))
        {
            return _configuration["ApiUrl"] + source.PictureUrl;
        }
        return null;
    }
}