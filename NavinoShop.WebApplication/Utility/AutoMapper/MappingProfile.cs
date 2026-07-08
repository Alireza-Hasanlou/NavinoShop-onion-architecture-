using AutoMapper;
using Query.Contract.UI.Cart;
using Shop.Application.Contract.Order.Command;

namespace NavinoShop.WebApplication.Utility.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CartUiQueryModel, ShopCartViewModel>();
        }
    }
}
