using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Application;
using Shared.Domain.Enums;
using Shared.Insfrastructure;
using Shop.Application.Contract.ProductSell.Command;
using Shop.Domain.ProductSellAgg;
using Shop.Infrastracture.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastracture.Persistence.Repository
{
    internal class ProductSellRepository : GenericRepository<ProductSell, int>, IProductSellRepository
    {
        private readonly ShopContext _shopContext;
        private IDbContextTransaction? _transaction;
        public ProductSellRepository(ShopContext context) : base(context)
        {
            _shopContext = context;
        }

        public async Task<OperationResult> EditProductSellAmountAsync(
            EditProductSellAmountCommandModel editAmountModel)
        {
            if (editAmountModel.count <= 0)
                return new(false, "تعداد باید بیشتر از صفر باشد.");

            int res;

            if (editAmountModel.Type == StoreProductType.افزایش)
            {
                res = await _shopContext.productSells
                    .Where(x => x.Id == editAmountModel.SellId)
                    .ExecuteUpdateAsync(setter =>
                        setter.SetProperty(
                            p => p.Amount,
                            x => x.Amount + editAmountModel.count));
            }
            else
            {
                res = await _shopContext.productSells
                    .Where(x =>
                        x.Id == editAmountModel.SellId &&
                        x.Amount >= editAmountModel.count)
                    .ExecuteUpdateAsync(setter =>
                        setter.SetProperty(
                            p => p.Amount,
                            x => x.Amount - editAmountModel.count));
            }

            if (res == 0)
                return new(false, "محصول پیدا نشد یا موجودی کافی نیست.");

            return new(true);
        }

        public async Task<List<ProductSell>> GetByIdsAsync(List<int> productSellIds)
        {
            return await _shopContext.productSells.Where(i => productSellIds.Any(x => x == i.Id)).ToListAsync();
        }

        public async Task<bool> ProductSellHaveAmount(int id, int quantity)
        {
            return await _shopContext.productSells.AnyAsync(x => x.Id == id && x.Amount >= quantity);
        }

    }
}
