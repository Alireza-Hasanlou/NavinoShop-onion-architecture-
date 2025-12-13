using Blogs.Domain.BlogAgg;
using Shared.Application;
using Shared.Application.Service;
using Blogs.Domain.BlogCategoryAgg;
using Shared.Application.Validations;
using Shared;
using Blogs.Application.Contract.BlogService.Command;

namespace Blogs.Application.Services
{
    internal class BlogService : IBlogCommandService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IFileService _fileService;
        private readonly IBlogCategoryRepository _blogCategoryRepository;

        public BlogService(IBlogRepository blogRepository, IFileService fileService, IBlogCategoryRepository blogCategoryRepository)
        {
            _blogRepository = blogRepository;
            _fileService = fileService;
            _blogCategoryRepository = blogCategoryRepository;
        }

        public async Task<OperationResult> ChangeActivationAsync(int id)
        {

            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return new OperationResult(false, "بلاگ پیدا نشد ", "Blog");
            }

            blog.ActivationChange();
            await _blogRepository.SaveAsync();

            return new OperationResult(true, "Blog");
        }

        public async Task<OperationResult> CreateAsync(CreateBlogCommand command)
        {
            if (await _blogRepository.ExistByAsync(t => t.Title == command.Title.Trim()))
            {
                return new OperationResult(false, ValidationMessages.DuplicatedMessage, nameof(command.Title));
            }
            var slug = command.Slug.GenerateSlug();
            if (await _blogRepository.ExistByAsync(t => t.Slug.Trim() == slug))
            {
                return new OperationResult(false, ValidationMessages.DuplicatedMessage, nameof(command.Slug));
            }
            if (command.ImageFile == null || !FileSecurity.IsImage(command.ImageFile))
            {
                return new OperationResult(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));
            }
            if (command.CategoryId < 1 ||
               await _blogCategoryRepository.ExistByAsync(c => c.Id == command.CategoryId && c.Parent == 0) == false)
                return new OperationResult(false, ValidationMessages.ParentCategoryMessage, nameof(command.CategoryId));

            if (command.SubCategoryId < 1 ||
               await _blogCategoryRepository.ExistByAsync(c => c.Id == command.SubCategoryId && c.Parent != 0) == false)
                return new OperationResult(false, ValidationMessages.ChildCategoryMessage, nameof(command.SubCategoryId));

            string imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.BlogImageFolder);
            if (imageName == "")
                return new(false, ValidationMessages.SystemErrorMessage, nameof(command.ImageFile));
            _fileService.ResizeImage(imageName, FileDirectories.BlogImageFolder, 400);
            _fileService.ResizeImage(imageName, FileDirectories.BlogImageFolder, 100);
            Blog blog = new Blog(command.Title, command.UserId, command.Writer,
                                 command.Slug, command.ShortDescription, command.Text,
                                 imageName, command.ImageAlt, command.CategoryId, command.SubCategoryId);


            var result = await _blogRepository.CreateAsync(blog);
            if (result.Success)
                return new(true);



            try { _fileService.DeleteImage($"{FileDirectories.BlogImageDirectory}{imageName}"); } catch { }
            try { _fileService.DeleteImage($"{FileDirectories.BlogImageDirectory400}{imageName}"); } catch { }
            try { _fileService.DeleteImage($"{FileDirectories.BlogImageDirectory100}{imageName}"); } catch { }

            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.ImageFile));
        }

        public async Task<OperationResult> EditAsync(EditBlogCommand command)
        {

            if (await _blogRepository.ExistByAsync(t => t.Title.Trim() == command.Title.Trim() && t.Id != command.Id))
                return new(false, ValidationMessages.DuplicatedMessage, command.Title);

            var slug = command.Slug.GenerateSlug();
            if (await _blogRepository.ExistByAsync(s => s.Slug.Trim() == slug && s.Id != command.Id))
                return new(false, ValidationMessages.DuplicatedMessage, command.Slug);

            if (command.CategoryId < 1 ||
              await _blogCategoryRepository.ExistByAsync(c => c.Id == command.CategoryId && c.Parent == 0) == false)
                return new OperationResult(false, ValidationMessages.ParentCategoryMessage, nameof(command.CategoryId));

            if (command.SubCategoryId < 1 ||
               await _blogCategoryRepository.ExistByAsync(c => c.Id == command.SubCategoryId && c.Parent != 0) == false)
                return new OperationResult(false, ValidationMessages.ChildCategoryMessage, nameof(command.SubCategoryId));
            if (command.ImageFile != null && !FileSecurity.IsImage(command.ImageFile))
                return new(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));
            string imageName = command.ImageName;
            string oldImagName = command.ImageName;
            if (command.ImageFile != null)
            {
                imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.BlogImageDirectory);
                if (imageName == "")
                    return new(false, ValidationMessages.SystemErrorMessage, nameof(command.ImageFile));
                _fileService.ResizeImage(imageName, FileDirectories.BlogImageFolder, 400);
                _fileService.ResizeImage(imageName, FileDirectories.BlogImageFolder, 100);
            }
            var blog = await _blogRepository.GetByIdAsync(command.Id);
            blog.Edit(command.Title, command.Writer, command.Slug, command.ShortDescription,
                      command.Text, imageName, command.ImageAlt, command.CategoryId, command.SubCategoryId);

            if (await _blogRepository.SaveAsync())
            {
                if (command.ImageFile != null)
                {

                    try { _fileService.DeleteImage($"{FileDirectories.BlogImageDirectory}{imageName}"); } catch { }
                    try { _fileService.DeleteImage($"{FileDirectories.BlogImageDirectory400}{imageName}"); } catch { }
                    try { _fileService.DeleteImage($"{FileDirectories.BlogImageDirectory100}{imageName}"); } catch { }
                }
                return new(true);
            }

            try { _fileService.DeleteImage($"{FileDirectories.BlogImageDirectory}{imageName}"); } catch { }
            try { _fileService.DeleteImage($"{FileDirectories.BlogImageDirectory400}{imageName}"); } catch { }
            try { _fileService.DeleteImage($"{FileDirectories.BlogImageDirectory100}{imageName}"); } catch { }

            return new(false, ValidationMessages.SystemErrorMessage, "Title");
        }

        public async Task<OperationResult> IncreaseVisitCountAsync(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            blog.VisitPlus();
            bool result = await _blogRepository.SaveAsync();
            return new(result);
        }
    }
}
