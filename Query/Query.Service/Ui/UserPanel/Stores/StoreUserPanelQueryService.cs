using Microsoft.EntityFrameworkCore;
using Query.Contract.UI.UserPanel.Stores;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductSellAgg;
using Shop.Domain.SellerAgg;
using Shop.Infrastracture.Persistence.Context;
using Store.Domain.StoreAgg;
using Store.Domain.StoreProductAgg;
using Store.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Service.Ui.UserPanel.Stores
{
    internal class StoreUserPanelQueryService : IStoreUserPanelQueryService
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly StoreContext _storeContext;
        private readonly ShopContext _shopContext;

        public StoreUserPanelQueryService(ISellerRepository sellerRepository, IStoreRepository storeRepository,
            StoreContext storeContext, ShopContext shopContext)
        {
            _sellerRepository = sellerRepository;
            _storeRepository = storeRepository;
            _storeContext = storeContext;
            _shopContext = shopContext;
        }

        public async Task<List<ProductSellsForAddToStoreQueryModel>> GetProductSellsForAddToStore(int SellerId)
        {
            return await _shopContext.productSells.Where(x => x.SellerId == SellerId && x.Active)
                .Include(p => p.Product)
                .Select(p => new ProductSellsForAddToStoreQueryModel
                {
                    Id = p.Id,
                    Title = p.Product.Title,
                    Count = p.Amount
                })
                .ToListAsync();


        }

        public async Task<StoreDetailsPaging> GetStoreDetails(int storeId)
        {
            var store = await _storeContext.Stores.Where(i => i.Id == storeId)
                .Include(p => p.StoreProducts)
                .SingleAsync();
            if (store == null) return null;



            if (!store.StoreProducts.Any())
                return new StoreDetailsPaging { StoreId = storeId, Description = store.Description, SellerId = store.SellerId, Details = new() };

            var productSellIds = store.StoreProducts.Select(sp => sp.ProdcutSellId).ToList();

            var productSells = await _shopContext.productSells
                .Where(ps => productSellIds.Contains(ps.Id))
                .Include(ps => ps.Product)
                .ToListAsync();

            var query = store.StoreProducts.Join(productSells,
                sp => sp.ProdcutSellId,
                ps => ps.Id,
                (sp, ps) => new StoreProductQueryModel
                {
                    CrateDate = sp.CreateDate,
                    ProdcutSellId = ps.Id,
                    Count = sp.Count,
                    ProductSellTitle = ps.Product.Title,
                    StoreProductType = sp.StoreProductType,

                });
            var model = new StoreDetailsPaging();
            var sellerTitle = _sellerRepository.GetTitleById(store.SellerId);
            model.Description = store.Description;
            model.StoreId = store.Id;
            model.Title = $"انبار فروشگاه {sellerTitle} ";
            model.SellerId = store.SellerId;
            model.Details = query.OrderByDescending(x => x.CrateDate).ToList();
            return model;
        }

        public async Task<List<StoresUserPanelQueryModel>> GetStores(int UserId)
        {
            return await _storeRepository.GetAllBy(x => x.UserId == UserId)
                .Select(s => new StoresUserPanelQueryModel
                {
                    Id = s.Id,
                    Title = _sellerRepository.GetTitleById(s.SellerId),
                    CreateDate = s.CreateDate,
                }).ToListAsync();
        }
    }
}
