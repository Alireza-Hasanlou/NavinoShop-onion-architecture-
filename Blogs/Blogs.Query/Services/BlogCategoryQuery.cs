using Blogs.Application.Contract.BlogApplication.Query;
using Blogs.Application.Contract.BlogCategoryService.Query;
using Blogs.Domain.BlogCategoryAgg;
using Microsoft.EntityFrameworkCore;
using Shared.Application;
using Utility.Shared.Application;

namespace Blogs.Query.Services
{
    internal class BlogCategoryQuery : IBlogCategoryQueryService
    {
        private readonly IBlogCategoryRepository _repository;

        public BlogCategoryQuery(IBlogCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> CheckCategoryHaveParentAsync(int id)
        {
            var blogCategory = await _repository.GetByIdAsync(id);
            if (blogCategory != null && blogCategory.Parent > 0)
                return true;
            return false;
        }

        public async Task<List<BlogCategoryForCreateBlogQueryModel>> GetCategoriesForAddBlogAsync(int id)
        {
            return await _repository.GetAllBy(i => i.Parent == id).Select(b => new BlogCategoryForCreateBlogQueryModel
            {
                Id = b.Id,
                Title = b.Title,
            }).ToListAsync();

        }

        public async Task<BlogCategoryAdminPageQueryModel> GetCategoriesForAdminAsync(int parentId)
        {
            BlogCategoryAdminPageQueryModel model = new()
            {
                parentId = parentId,

                Categories = await _repository.GetAllBy(i => i.Parent == parentId).Select(b => new BlogCategoryAdminQueryModel
                {
                    Id = b.Id,
                    ParentId=b.Parent,
                    Title = b.Title,
                    Active = b.Active,
                    CreationDate = b.CreateDate.ToPersainDate(),
                    ImageName = FileDirectories.BlogCategoryImageDirectory100+b.ImageName,
                    UpdateDate = b.UpdateDate.ToPersainDate(),
                }).ToListAsync()

            };



            if (parentId > 0)
            {
                var blogCategory = await _repository.GetByIdAsync(parentId);
                model.PageTitle = $" لیست زیردسته های  {blogCategory.Title}";
            }
            else
            {
                model.PageTitle = $"لیست سر دسته های مقاله";
            }

            return model;
        }

        public async Task<EditBlogCategoryDto?> GetForEditAsync(int id)
        {
            var blogCategory = await _repository.GetByIdAsync(id);
            if (blogCategory == null)
                return null;

            return new EditBlogCategoryDto
            {
                Id = blogCategory.Id,
                Title = blogCategory.Title,
                Slug = blogCategory.Slug,
                Parent = blogCategory.Parent,
                ImageAlt = blogCategory.ImageAlt,
                ImageName =  blogCategory.ImageName,
                ImageFile = null
            };
        }

    }
}
