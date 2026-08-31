using Shop.Application.Contract.OrderSeller.Command;
using Shop.Domain.OrderAgg;
using Shop.Domain.OrderSellerAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Commands
{
    public class OrderSellerCommands : IOrderSellerCommands
    {
        private readonly IOrderSellerRepository _orderSellerRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderSellerCommands(IOrderSellerRepository orderSellerRepository, IOrderRepository orderRepository)
        {
            _orderSellerRepository = orderSellerRepository;
            _orderRepository = orderRepository;
        }

        public async Task<PricesAfterAddPost> AddPostToSellerAsync(AddPostToSellerDto addPostToSellerDto)
        {
            var order = await _orderRepository.GetOpenOrderForUserAsync(addPostToSellerDto.userId);
            if (order == null)
                return new PricesAfterAddPost { Success = false, Message = "فاکتوری برای شما یافت نشد " };
            var seller = order.OrderSellers.FirstOrDefault(x => x.Id == addPostToSellerDto.orderSellerId);
            if (seller == null)
                return new PricesAfterAddPost { Success = false, Message = "داده های نامعتبر!" };
            seller.AddPostPrice(addPostToSellerDto.postPrice, addPostToSellerDto.postId, addPostToSellerDto.postTitle);
            if (await _orderSellerRepository.SaveAsync())
            {
               
                return new PricesAfterAddPost
                { 
                    Success = true, 
                    Message = $" محصول شما باپست {addPostToSellerDto.postTitle } ارسال خواهد شد ",
                    TotalPostPrice = order.PostPrice,
                    TotalPrice= order.Price ,
                    PostPrice=seller.PostPrice,
                    PostTitle=seller.PostTitle,
                    TotalPriceAfterOff = order.PriceAfterOff ,
                    PaymentPrice = order.PaymentPrice ,
                };
            }


            return new PricesAfterAddPost { Success = false, Message = "خطا در انتخاب روش ارسال" };

        }
    }
}
