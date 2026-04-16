using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.Admin.Seller
{
    public interface IAdminSellerQueryService
    {
        Task<List<SellersRequrstAdminQueryModel>> GetAllSalesRequrstForAdmin();
        Task<List<SellersRequrstAdminQueryModel>> GetAllSellersForAdmin();
        Task<List<SellersRequrstAdminQueryModel>> GetAllRejectedRequestsForAdmin();
        Task<SellerRequestDetailQueryModel> GetSellerRequestDetail(int Id);

    }
}
