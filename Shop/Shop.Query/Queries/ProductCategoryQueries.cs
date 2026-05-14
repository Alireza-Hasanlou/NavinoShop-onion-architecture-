using Microsoft.EntityFrameworkCore;
using Shared.Application;
using Shop.Application.Contract.ProductCategory.Query;
using Shop.Domain.ProductCategoryAgg;
using System.Collections.Generic;
using System.Collections.Immutable;
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



        public async Task<List<CategoryTreeItem>> GetCategoriesForAddProduct()
        {
            var productCategories = await _productCategoryRepository.GetAll().ToListAsync();

            if (productCategories == null || !productCategories.Any())
                return new List<CategoryTreeItem>();

            var lookup = productCategories.ToLookup(c => c.ParentId);

            List<CategoryTreeItem> BuildTree(int parentId, int level = 0)
            {
                return lookup[parentId]
                    .Select(c => new CategoryTreeItem
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Slug = c.Slug,
                        ParentId = c.ParentId,
                        Level = level,
                        HasChildren = lookup[c.Id].Any(),
                        IsChecked = false,
                        Children = BuildTree(c.Id, level + 1) 
                    })
                    .ToList();
            }
            var categoryTree = BuildTree(0, 0);

            return categoryTree;

        }






        public async Task<ProductCategoryAdminPageQueryModel> GetCategoriesForAdmin(int id)
        {
            var model = new ProductCategoryAdminPageQueryModel();
            var productCategories = _productCategoryRepository.GetAll();
            if (id < 1)
            {
                model.Title = "همه دسته بندی ها ";
                model.productCategories = await productCategories
                    .Where(p => p.ParentId == 0)
                    .OrderByDescending(c => c.CreateDate)
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




    }


}
