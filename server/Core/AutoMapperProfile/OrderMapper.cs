using AutoMapper;
using Ecommerce.Module.Orders.Model;

namespace Ecommerce.Core.AutoMapperProfile;

public class OrderMapper : Profile
{
    protected OrderMapper()
    {
        CreateMap<AddressDTO, Address>();
    }
}