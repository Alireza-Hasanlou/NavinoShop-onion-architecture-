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

            string ImageName = _fileService.UploadImage(command.ImageFile, FileDirectories.BlogImageDirectory);
            _fileService.ResizeImage( $"{FileDirectories.BlogImageDirectory}{ImageName}",$"{FileDirectories.BlogImageDirectory100}{ImageName}",100);
            _fileService.ResizeImage($"{FileDirectories.BlogImageDirectory}{ImageName}", $"{FileDirectories.BlogImageDirectory400}{ImageName}", 400);
            Blog blog = new Blog(command.Title, command.UserId, command.Writer,
                                 command.Slug, command.ShortDescription, command.Text,
                                 ImageName, command.ImageAlt, command.CategoryId, command.SubCategoryId);


            return await _blogRepository.CreateAsync(blog);

        }

        public Task<OperationResult> EditAsync(EditBlogCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> IncreaseVisitCountAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
