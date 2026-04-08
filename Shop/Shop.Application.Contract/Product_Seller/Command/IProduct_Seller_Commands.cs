using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.Product_Seller.Command
{
    public partial interface IProduct_Seller_Commands
    {
        Task<OperationResult> CreateAsync(CreateProductSellerCommandModel command);
        Task<OperationResult> EditAsync(EditProductSellerCommandModel command);
        Task<EditProductSellerCommandModel> GetForEditAsync(int id);
        Task<OperationResult> ActivationChangeAsync(int id);
        Task<OperationResult> EditProductSellAmountAsync(List<EditProductSellerAmountCommandModel> sels);
    }
