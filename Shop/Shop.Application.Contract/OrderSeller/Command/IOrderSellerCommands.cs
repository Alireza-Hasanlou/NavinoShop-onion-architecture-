using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.OrderSeller.Command;
public interface IOrderSellerCommands
{
    Task<PricesAfterAddPost> AddPostToSellerAsync(AddPostToSellerDto addPostToSellerDto);
}

public record AddPostToSellerDto(int userId, int orderId, int orderSellerId, int postId, int postPrice, string postTitle);
public class PricesAfterAddPost
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public int TotalPostPrice { get; set; }
    public int  PostPrice { get; set; }
    public string PostTitle  { get; set; }
    public int TotalPrice { get; set; }
    public int TotalPriceAfterOff { get; set; }
    public int  PaymentPrice { get; set; }
}