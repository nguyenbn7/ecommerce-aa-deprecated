using AutoMapper;
using Ecommerce.Core.AutoMapperResolver;
using Ecommerce.Module.Products.DTO;
using Ecommerce.Module.Products.Model;

namespace Ecommerce.Core.AutoMapperProfile;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<Product, ProductReponse>()
            .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
            .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductImageUrl>());
    }
}