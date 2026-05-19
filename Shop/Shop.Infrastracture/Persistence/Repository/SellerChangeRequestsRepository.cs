using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using Shop.Application.Contract.Seller.Query;
using Shop.Domain.SellerChangeRequestsAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class SellerChangeRequestsRepository : GenericRepository<SellerChangeRequest, int>, ISellerChangeRequestsRepository
    {
        private readonly ShopContext _context;

        public SellerChangeRequestsRepository(ShopContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SellersChangeRequestQueryModel>> GetChangeRequests()
        {
            return await _context.SellerChangeRequests.Include(x => x.Seller)
                .OrderByDescending(x=>x.RequestDate)
                .Select(s => new SellersChangeRequestQueryModel
                {
                    Id=s.Id,
                    WhyRejected=s.WhyRejected,
                    SellerTitle = s.Seller.Title,
                    Address = s.Seller.Address,
                    Email = s.Seller.Email,
                    ImageName = s.Seller.ImageName,
                    PhoneNumber = s.Seller.Phone1 + "-" + s.Seller.Phone2,
                    RequestDate = s.RequestDate,
                    SellerId = s.SellerId,
                    Status = s.status,
                }).ToListAsync();
        }
    }
}
