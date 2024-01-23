using AutoMapper;
using Ecommerce.Module.Products.DTO;
using Ecommerce.Module.Products.Model;

namespace Ecommerce.Core.AutoMapperResolver;

public class ProductImageUrl : IValueResolver<Product, ProductReponse, string?>
{
    private readonly IConfiguration _configuration;

    public ProductImageUrl(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string? Resolve(Product source,
                           ProductReponse destination,
                           string? destMember,
                           ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.PictureUrl))
        {
            return _configuration["ApiUrl"] + source.PictureUrl;
        }
        return null;
    }
}