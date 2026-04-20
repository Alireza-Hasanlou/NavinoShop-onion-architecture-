using Microsoft.EntityFrameworkCore;
using Shared.Application;
using Shop.Application.Contract.ProductCategory.Query;
using Shop.Domain.ProductCategoryAgg;
using System.Threading.Tasks;


namespace Shop.Query.Queries
{
    internal class ProductCategoryQueries : IProductCategoryQueries
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryQueries(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<bool> CheckCategoryHaveParent(int id)
        {
            var productCategory = await _productCategoryRepository.GetByIdAsync(id);
            return productCategory != null && productCategory.ParentId > 0 ? true : false;
        }

        public async Task<List<ProductCategoryForAddProduct>> GetCategoriesForAddProduct()
        {
            return await _productCategoryRepository.GetAll()
                .Select(x => new ProductCategoryForAddProduct
                {
                    Id = x.Id,
                    Parent = x.ParentId,
                    Title = x.Title,
                })
                .ToListAsync();
        }

        public async Task<ProductCategoryAdminPageQueryModel> GetCategoriesForAdmin(int id)
        {
            var model = new ProductCategoryAdminPageQueryModel();
            var productCategories = _productCategoryRepository.GetAll();
            if (id < 1)
            {
                model.Title = "همه دسته بندی ها ";
                model.productCategories = await productCategories
                    .Where(p=>p.ParentId==0)
                    .Select(x => new ProductCategoryQueryModel
                {
                    Id = x.Id,
                    ParentId = x.ParentId,
                    Title = x.Title,
                    Active = x.Active,
                    CreationDate = x.CreateDate.ToPersainDate(),
                    UpdateDate = x.UpdateDate.ToPersainDate(),
                }).ToListAsync();

            }
            else
            {
                model.ProductCategoryId = id;
                var parent = await _productCategoryRepository.GetByIdAsync(id);
                model.Title = $"نمایش زیرگروه های دسته بندی {parent.Title}";
                model.productCategories = await productCategories.Where(p => p.ParentId == parent.Id)
               .Select(x => new ProductCategoryQueryModel
               {
                   Id = x.Id,
                   ParentId = x.ParentId,   
                   Title = x.Title,
                   Active = x.Active,
                   CreationDate = x.CreateDate.ToPersainDate(),
                   UpdateDate = x.UpdateDate.ToPersainDate(),
               }).ToListAsync();

            }

            return model;
        }

        public async Task<List<ProductCategoryForAddProductSeller>> GetCategoryForAddProductSells(int id)
        {
            return await _productCategoryRepository.GetAll()
               .Select(x => new ProductCategoryForAddProductSeller
               {
                   Id = x.Id,
                   Title = x.Title,
               })
               .ToListAsync();
        }


    }
}
