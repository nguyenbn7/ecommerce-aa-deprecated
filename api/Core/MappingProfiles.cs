using AutoMapper;
using Ecommerce.API.Accounts;
using Ecommerce.API.Baskets;
using Ecommerce.API.Products;
using Ecommerce.Core.Resolvers;

namespace Ecommerce.Core.MappingProfiles;

public class BasketProfile : Profile
{
    public BasketProfile()
    {
        CreateMap<CustomerBasketDTO, CustomerBasket>();
        CreateMap<BasketItemDTO, BasketItem>();
    }
}

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDTO>()
            .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
            .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductURLResolver>());
    }
}

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<Address, AddressDTO>().ReverseMap();
    }
}