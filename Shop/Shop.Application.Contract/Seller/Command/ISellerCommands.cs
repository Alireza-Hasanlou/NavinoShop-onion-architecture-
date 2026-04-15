using Shared.Application;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Contract.Seller.Command
{
    public interface ISellerCommands
    {
        Task<OperationResult> RequestForSales(int UserId, RequestForSelasCommandModel command);
        Task<OperationResult> EditRequestForSales( EditRequestForSelasCommandModel command);
        Task<EditRequestForSelasCommandModel> GetForEditRequestForSales(int Id);
        Task<OperationResult> ChangeSellerStatus(int Id,SellerStatus sellerStatus,string ? whyRejected );
    }
}
