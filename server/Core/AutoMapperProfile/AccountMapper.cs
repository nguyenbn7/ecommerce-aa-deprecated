using AutoMapper;
using Ecommerce.Module.Accounts.DTO;
using Ecommerce.Module.Accounts.Model;

namespace Ecommerce.Core.AutoMapperProfile;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<Address, CustomerAddress>().ReverseMap();
    }
}