using AutoMapper;
using Query.Contract.UI.Cart;
using Shop.Application.Contract.Order.Command;
using Users.Application.Contract.UserAddressService.Query;

namespace NavinoShop.WebApplication.Utility.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CartUiQueryModel, ShopCartViewModel>();
            CreateMap<UserAddressForOrderQueryModel, UpsertOrderAddressCommandModel>();
        }
    }
}
