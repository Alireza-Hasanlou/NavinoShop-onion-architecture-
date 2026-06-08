using Discount.Domain.ProductDiscountAgg;
using Microsoft.EntityFrameworkCore;
using Query.Contract.Admin.Products;
using Query.Contract.UI.UserPanel.Seller;
using Shared.Domain.Enums;
using Shop.Domain.OrderItemAgg;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductCategoryAgg;
using Shop.Domain.ProductSellAgg;
using Shop.Domain.Relations.ProductCategoryRel;


namespace Query.Service.Admin.Products
{
    internal class AdminProductsQueryService : IAdminProductsQueryService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProduct_Category_Repository _product_Category_Repository;
        private readonly IProductSellRepository _productSellRepository;
        private readonly IOrderItemRepository _orderItemReposotory;
        private readonly IProductDiscountRepository _productDiscountRepository;

        public AdminProductsQueryService(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository,
            IProduct_Category_Repository product_Category_Repository, IProductSellRepository productSellRepository,
            IOrderItemRepository orderItemReposotory, IProductDiscountRepository productDiscountRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _product_Category_Repository = product_Category_Repository;
            _productSellRepository = productSellRepository;
            _orderItemReposotory = orderItemReposotory;
            _productDiscountRepository = productDiscountRepository;
        }

        public async Task<ProductsForAdminPaging> GetAllProductsForAdmin(int pageId, int take, string filter, int categoryId)
        {
            var model = new ProductsForAdminPaging();
            var products = _productRepository.GetAll().Distinct();


            if (categoryId > 0)
            {
                var category = await _productCategoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                    return new ProductsForAdminPaging();

                var productIds = _product_Category_Repository.GetAllBy(x => x.CategoryId == categoryId)
                    .Select(x => x.ProductId);

                products = products.Where(p => productIds.Contains(p.Id));
                model.Title = $"محصولات در دسته بندی {category.Title}";
                model.CategoryId = categoryId;
            }


            if (!string.IsNullOrEmpty(filter))
            {
                products = products.Where(p =>

                   p.Title.Contains(filter)
                || p.Description.Contains(filter)
                || p.Slug.Contains(filter));
            }


            model.GetData(products, pageId, take, 5);
            model.Filter = filter ?? "";

            model.products = await products
                .OrderByDescending(p => p.CreateDate)
                .Skip(model.Skip)
                .Take(model.Take)
                .Select(p => new ProductsForAdminQueryModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    ShortDescription = p.ShortDescription,
                    ImageName = p.ImageName,
                    CreateDate = p.CreateDate,
                    UpdateDate = p.UpdateDate,
                    Slug = p.Slug,
                    Active = p.Active,
                    Weight = p.Weight,
                })
                .ToListAsync();

            var ProductDiscountPercents = await _productDiscountRepository.GetProductsDiscountAsync();
            foreach (var product in model.products)
            {
                product.DiscountPercent = ProductDiscountPercents.FirstOrDefault(x => x.ProductId == product.Id && x.ProductSellId==0)?.Percent ?? 0;
            }

            return model;
        }
  
    }
}
