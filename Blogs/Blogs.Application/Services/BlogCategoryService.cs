using Blogs.Application.Contract.BlogCategoryService.Command;
using Blogs.Domain.BlogAgg;
using Blogs.Domain.BlogCategoryAgg;
using Shared.Application;
using Shared.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Shared;
using Utility.Shared.Application;

namespace Blogs.Application.Services
{
    internal class BlogCategoryService : IBlogCategoryCommandService
    {
        private readonly IBlogCategoryRepository _blogCategoryRepository;
        private readonly IFileService _fileService;

        public BlogCategoryService(IBlogCategoryRepository blogCategoryRepository, IFileService fileService)
        {
            _blogCategoryRepository = blogCategoryRepository;
            _fileService = fileService;
        }

        public async Task<OperationResult> ActivationChange(int id)
        {
            var blogcategory = await _blogCategoryRepository.GetByIdAsync(id);
            blogcategory.ActivationChange();
            var result = await _blogCategoryRepository.SaveAsync();
            return new(result);
        }

        public async Task<OperationResult> Create(CreateBlogCategoryCommand command)
        {
            if (await _blogCategoryRepository.ExistByAsync(t => t.Title == command.Title.Trim()))
            {
                return new OperationResult(false, ValidationMessages.DuplicatedMessage, nameof(command.Title));
            }
            var slug = command.Slug.GenerateSlug();
            if (await _blogCategoryRepository.ExistByAsync(t => t.Slug.Trim() == slug))
            {
                return new OperationResult(false, ValidationMessages.DuplicatedMessage, nameof(command.Slug));
            }
            if (command.ImageFile == null || !FileSecurity.IsImage(command.ImageFile))
            {
                return new OperationResult(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));
            }

            string imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.BlogCategoryImageFolder);
            if (imageName == "")
                return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));
            _fileService.ResizeImage(imageName, $"{FileDirectories.BlogCategoryImageFolder}", 100);
            _fileService.ResizeImage(imageName, $"{FileDirectories.BlogCategoryImageFolder}", 400);

            BlogCategory blogCategory = new(command.Title, slug, imageName, command.ImageAlt, command.Parent);

            var reslut = await _blogCategoryRepository.CreateAsync(blogCategory);
            if (reslut.Success)
                return new(true);

            try { _fileService.DeleteImage(Path.Combine(FileDirectories.BlogCategoryImageDirectory, imageName)); } catch { }
            try { _fileService.DeleteImage(Path.Combine(FileDirectories.BlogCategoryImageDirectory400, imageName)); } catch { }
            try { _fileService.DeleteImage(Path.Combine(FileDirectories.BlogCategoryImageDirectory100, imageName)); } catch { }

            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));

        }

        public async Task<OperationResult> Edit(EditBlogCategoryCommand command)
        {


            if (await _blogCategoryRepository.ExistByAsync(b => b.Title.Trim() == command.Title.Trim() && b.Id != command.Id))
                return new(false, ValidationMessages.DuplicatedMessage, command.Title);

            var slug = command.Slug.GenerateSlug();
            if (await _blogCategoryRepository.ExistByAsync(t => t.Slug.Trim() == slug && t.Id != command.Id))
            {
                return new OperationResult(false, ValidationMessages.DuplicatedMessage, "Slug");
            }
            if (command.ImageFile != null && !FileSecurity.IsImage(command.ImageFile))
            {
                return new OperationResult(false, ValidationMessages.ImageErrorMessage, "Image");
            }
            string imageName = command.ImageName;
            string oldImagName = command.ImageName;
            if (command.ImageFile != null)
            {
                imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.BlogCategoryImageFolder);
                if (imageName == "")
                    return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));
                _fileService.ResizeImage(imageName, $"{FileDirectories.BlogCategoryImageFolder}", 100);
                _fileService.ResizeImage(imageName, $"{FileDirectories.BlogCategoryImageFolder}", 400);
            }
            var blogCategory = await _blogCategoryRepository.GetByIdAsync(command.Id);
            blogCategory.Edit(command.Title, slug, imageName, command.ImageAlt);

            if (await _blogCategoryRepository.SaveAsync())
            {
                if (command.ImageFile != null)
                {

                    try { _fileService.DeleteImage(Path.Combine(FileDirectories.BlogCategoryImageDirectory, oldImagName)); } catch { }
                    try { _fileService.DeleteImage(Path.Combine(FileDirectories.BlogCategoryImageDirectory400, oldImagName)); } catch { }
                    try { _fileService.DeleteImage(Path.Combine(FileDirectories.BlogCategoryImageDirectory100, oldImagName)); } catch { }

                }
                return new(true);
            }

            return new(false, ValidationMessages.SystemErrorMessage, nameof(blogCategory.Title));

        }
    }
}
