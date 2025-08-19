using Blogs.Application.Contract.BlogApplication.Command;
using Blogs.Domain.BlogAgg;
using Utility.Shared;
using Shared.Application;
using Shared.Application.Service;

using Utility.Shared.Application;

namespace Blogs.Application.Services
{
    internal class BlogService : IBlogCommandService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IFileService _fileService;

        public BlogService(IBlogRepository blogRepository, IFileService fileService)
        {
            _blogRepository = blogRepository;
            _fileService = fileService;
        }

        public async Task<OperationResult> ChangeActivationAsync(int id)
        {

            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return new OperationResult(false, "Blog is Not found ", "Blog");
            }

            blog.ActivationChange();
            return new OperationResult(true, "ActivationChanged ", "Blog");
        }

        public async Task<OperationResult> CreateAsync(CreateBlogCommand command)
        {
            if (await _blogRepository.ExistByAsync(t => t.Title == command.Title.Trim()))
            {
                return new OperationResult(false, ValidationMessages.DuplicatedMessage, "Title");
            }
            var slug = SlugUtility.GenerateSlug(command.Slug);
            if (await _blogRepository.ExistByAsync(t => t.Slug.Trim() == slug))
            {
                return new OperationResult(false, ValidationMessages.DuplicatedMessage, "Slug");
            }
            if (command.ImageFile == null || !FileSecurity.IsImage(command.ImageFile))
            {
                return new OperationResult(false, ValidationMessages.ImageErrorMessage, "Image");
            }

            string imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.BlogImageDirectory);
            if (imageName == "")
                return new(false, ValidationMessages.SystemErrorMessage, "Title");
            _fileService.ResizeImage($"{FileDirectories.BlogImageDirectory}{imageName}", $"{FileDirectories.BlogImageDirectory100}{imageName}", 100);
            _fileService.ResizeImage($"{FileDirectories.BlogImageDirectory}{imageName}", $"{FileDirectories.BlogImageDirectory400}{imageName}", 400);
            Blog blog = new Blog(command.Title, command.UserId, command.Writer,
                                 command.Slug, command.ShortDescription, command.Text,
                                 imageName, command.ImageAlt, command.CategoryId, command.SubCategoryId);


            var result = await _blogRepository.CreateAsync(blog);
            if (result.Success)
                return new(true);

            try { _fileService.DeleteImage(Path.Combine(FileDirectories.BlogImageDirectory, imageName)); } catch { }
            try { _fileService.DeleteImage(Path.Combine(FileDirectories.BlogImageDirectory400, imageName)); } catch { }
            try { _fileService.DeleteImage(Path.Combine(FileDirectories.BlogImageDirectory100, imageName)); } catch { }

            return new(false, ValidationMessages.SystemErrorMessage, "Title");
        }

        public async Task<OperationResult> EditAsync(EditBlogCommand command)
        {

            if (await _blogRepository.ExistByAsync(t => t.Title.Trim() == command.Title.Trim() && t.Id != command.Id))
                return new(false, ValidationMessages.DuplicatedMessage, command.Title);

            var slug = SlugUtility.GenerateSlug(command.Slug);
            if (await _blogRepository.ExistByAsync(s => s.Slug.Trim() == slug && s.Id != command.Id))
                return new(false, ValidationMessages.DuplicatedMessage, command.Slug);

            if (command.ImageFile != null && !FileSecurity.IsImage(command.ImageFile))
                return new(false, ValidationMessages.ImageErrorMessage, "ImageFile");
            string imageName = command.ImageName;
            string oldImagName = command.ImageName;
            if (command.ImageFile != null)
            {
                imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.BlogImageDirectory);
                if (imageName == "")
                    return new(false, ValidationMessages.SystemErrorMessage, "Title");
                _fileService.ResizeImage($"{FileDirectories.BlogImageDirectory}{imageName}", $"{FileDirectories.BlogImageDirectory100}{imageName}", 100);
                _fileService.ResizeImage($"{FileDirectories.BlogImageDirectory}{imageName}", $"{FileDirectories.BlogImageDirectory400}{imageName}", 400);
            }
            var blog = await _blogRepository.GetByIdAsync(command.Id);
            blog.Edit(command.Title, command.Writer, command.Slug, command.ShortDescription,
                      command.Text, imageName, command.ImageAlt, command.CategoryId, command.SubCategoryId);

            if (await _blogRepository.SaveAsync())
            {
                if (command.ImageFile != null)
                {
                    try { _fileService.DeleteImage(Path.Combine(FileDirectories.BlogImageDirectory, oldImagName)); } catch { }
                    try { _fileService.DeleteImage(Path.Combine(FileDirectories.BlogImageDirectory400, oldImagName)); } catch { }
                    try { _fileService.DeleteImage(Path.Combine(FileDirectories.BlogImageDirectory100, oldImagName)); } catch { }
                }
                return new(true);
            }
            try { _fileService.DeleteImage(Path.Combine(FileDirectories.BlogImageDirectory, imageName)); } catch { }
            try { _fileService.DeleteImage(Path.Combine(FileDirectories.BlogImageDirectory400, imageName)); } catch { }
            try { _fileService.DeleteImage(Path.Combine(FileDirectories.BlogImageDirectory100, imageName)); } catch { }

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
