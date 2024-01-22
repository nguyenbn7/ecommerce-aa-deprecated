using AutoMapper;
using Ecommerce.Module.Baskets.DTO;
using Ecommerce.Module.Baskets.Model;

namespace Ecommerce.Core.AutoMapperProfile;

public class BasketMapper : Profile
{
    public BasketMapper()
    {
        CreateMap<CustomerBasket, Basket>();
        CreateMap<CustomerBasketItem, BasketItem>();
    }
}