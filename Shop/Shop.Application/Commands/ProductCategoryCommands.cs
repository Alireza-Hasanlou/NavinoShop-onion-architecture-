using Shared;
using Shared.Application;
using Shared.Application.Service;
using Shared.Application.Validations;
using Shop.Application.Contract.ProductCategory.Commands;
using Shop.Domain.ProductCategoryAgg;



namespace Shop.Application.Commands
{
    internal class ProductCategoryCommands: IProductCategoryCommands
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IFileService _fileService;

        public ProductCategoryCommands(IProductCategoryRepository productCategoryRepository, IFileService fileService)
        {
            _productCategoryRepository = productCategoryRepository;
            _fileService = fileService;
        }

        public async Task<OperationResult> ChangeActivation(int productCategoryId)
        {

            var productCategory = await _productCategoryRepository.GetByIdAsync(productCategoryId);
            productCategory.ActivationChange();
            if (await _productCategoryRepository.SaveAsync())
                return new OperationResult(true);
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> CreateAsync(CreateProductCategoryCommandModel command)
        {

            if (await _productCategoryRepository.ExistByAsync(t => t.Title.Trim().ToLower() == command.Title.Trim().ToLower()))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage);


            if (await _productCategoryRepository.ExistByAsync(s => s.Slug.Trim().ToLower() == command.Slug.Trim().ToLower()))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage);

            if (command.ImageFile == null || !command.ImageFile.IsImage())
                return new OperationResult(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

            var imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.ProductCategoryImageFolder);
            if (!string.IsNullOrEmpty(imageName))
            {
                _fileService.ResizeImage(imageName, FileDirectories.ProductCategoryImageFolder, 100);
                _fileService.ResizeImage(imageName, FileDirectories.ProductCategoryImageFolder, 500);

            }
            var newProductCategory = new ProductCategory(command.Title, imageName, command.ImageAlt.Trim().ToLower(), command.Slug.Trim().ToLower(),command.ParentId);

            var res = await _productCategoryRepository.CreateAsync(newProductCategory);
            if (res.Success)
                return new OperationResult(true);


            if (command.ImageFile != null)
            {
                _fileService.DeleteImage(FileDirectories.ProductCategoryImageDirectory);
                _fileService.DeleteImage(FileDirectories.ProductCategoryImageDirectory100);
                _fileService.DeleteImage(FileDirectories.ProductCategoryImageDirectory500);

            }
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> EditAsync(EditProductCategoryCommandModel command)
        {

            var productCategory = await _productCategoryRepository.GetByIdAsync(command.ProductCategoryId);
            if (productCategory == null)
                return new OperationResult(false, "دسته بندی مورد نظر یافت نشد");

            if (await _productCategoryRepository.ExistByAsync(t => t.Title == command.Title && t.Id != command.ProductCategoryId))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage);


            if (await _productCategoryRepository.ExistByAsync(s => s.Slug == command.Slug && s.Id != command.ProductCategoryId))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage);

           
            var OldimageName = command.ImageName;
            var NewImageName = command.ImageName;
            if (command.ImageFile != null)
            {
                if (!command.ImageFile.IsImage())
                    return new OperationResult(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

                NewImageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.ProductCategoryImageFolder);
                if (!string.IsNullOrEmpty(NewImageName))
                {
                    _fileService.ResizeImage(NewImageName, FileDirectories.ProductCategoryImageFolder, 100);
                    _fileService.ResizeImage(NewImageName, FileDirectories.ProductCategoryImageFolder, 500);

                }

            }
            productCategory.Edit(command.Title,NewImageName,command.ImageAlt,command.Slug,command.ParentId);
            if (await _productCategoryRepository.SaveAsync())
            {

                if (command.ImageFile != null)
                {
                    _fileService.DeleteImage($"{FileDirectories.ProductImageDirectory}{OldimageName}");
                    _fileService.DeleteImage($"{FileDirectories.ProductImageDirectory500}{OldimageName}");
                    _fileService.DeleteImage($"{FileDirectories.ProductImageDirectory100}{OldimageName}");

                }
                return new OperationResult(true);
            }

            _fileService.DeleteImage($"{FileDirectories.ProductImageDirectory}{NewImageName}");
            _fileService.DeleteImage($"{FileDirectories.ProductImageDirectory500}{NewImageName}");
            _fileService.DeleteImage($"{FileDirectories.ProductImageDirectory100}{NewImageName}");

            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<EditProductCategoryCommandModel> GetForEditAsync(int productCategoryId)
        {
            return await _productCategoryRepository.GetForEditAsync(productCategoryId); 
        }
    }
}
