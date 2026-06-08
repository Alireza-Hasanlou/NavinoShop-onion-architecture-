using Shop.Application.Contract.ProductSell.Query;
using Shop.Domain.ProductSellAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Query.Queries
{
    internal class ProductSellQueries : IProductSellQueries
    {
        private readonly IProductSellRepository _productSellRepository;

        public ProductSellQueries(IProductSellRepository productSellRepository)
        {
            _productSellRepository = productSellRepository;
        }

        public async Task<bool> ProductSellHaveAmount(int Id)
        {
           return await _productSellRepository.ProductSellHaveAmount(Id);   
        }
    }
}
