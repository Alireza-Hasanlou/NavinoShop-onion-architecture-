using Microsoft.EntityFrameworkCore;
using Shared.Domain.Enums;
using Shop.Application.Contract.Product.Query;
using Shop.Domain.OrderAgg;
using Shop.Domain.OrderItemAgg;
using Shop.Domain.OrderSellerAgg;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductCategoryAgg;
using Shop.Domain.ProductSellAgg;
using Shop.Domain.Relations.ProductCategoryRel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Query.Queries
{
    internal class ProductQueries : IProductQueries
    {
        private readonly IProductRepository _productRepository;

        public ProductQueries(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<List<ProductsForAddtoShopQueryModel>> GetProductsForAddToShop(List<int> categoryIds)
        {
            return _productRepository.GetProductsForAddToShop(categoryIds);
        }

   
    }
}

