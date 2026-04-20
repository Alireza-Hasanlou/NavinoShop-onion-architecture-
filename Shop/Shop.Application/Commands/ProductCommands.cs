using Shared;
using Shared.Application;
using Shared.Application.Service;
using Shared.Application.Validations;
using Shop.Application.Contract.Product.Command;
using Shop.Application.Contract.ProductCategory.Query;
using Shop.Domain.ProductAgg;
using Shop.Domain.ProductCategoryAgg;
using Shop.Domain.Relations.ProductCategoryRel;


namespace Shop.Application.Commands
{
    internal class ProductCommands : IProductCommands
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;
        private readonly IProductCategoryQueries _productCategoryQueries;

        public ProductCommands(IProductRepository productRepository, IFileService fileService, IProductCategoryQueries productCategoryQueries)
        {
            _productRepository = productRepository;
            _fileService = fileService;
            _productCategoryQueries = productCategoryQueries;
        }

        public async Task<OperationResult> ChangeActivation(int productId)
        {

            var product = await _productRepository.GetByIdAsync(productId);
            product.ActivationChange();
            if (await _productRepository.SaveAsync())
                return new OperationResult(true);
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> CreateAsync(CreateProductCommandModel command)
        {

            if (await _productRepository.ExistByAsync(t => t.Title.Trim().ToLower() == command.Title.Trim().ToLower()))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage);


            if (await _productRepository.ExistByAsync(s => s.Slug.Trim().ToLower() == command.Slug.Trim().ToLower()))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage);

            if (command.categoryIds?.Count < 1)
                return new OperationResult(false, "لطفا حداقل یک دسته بندی برای محصول انتخاب کنید");

            if (command.Weight < 1)
                return new OperationResult(false, "وزن محصول نمیتواند کمتر از 1 گرم باشد");

            if (command.ImageFile == null || !command.ImageFile.IsImage())
                return new OperationResult(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

            var imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.ProductImageFolder);
            if (string.IsNullOrEmpty(imageName))
                return new OperationResult(false, "خطا در بازگذاری تصویر", nameof(command.ImageFile));

            _fileService.ResizeImage(imageName, FileDirectories.ProductImageFolder, 100);
            _fileService.ResizeImage(imageName, FileDirectories.ProductImageFolder, 500);

            var newProduct = new Product(command.Title.Trim().ToLower(), imageName, command.ImageAlt.Trim().ToLower(), command.ShortDescription,
                command.Text, command.Weight, command.Slug.Trim().ToLower());

            //TODO how to add parent ID
            var rels = new List<Product_Category_Rel>();
            foreach (var item in command.categoryIds)
            {
                rels.Add(new Product_Category_Rel(item));
            }

            newProduct.EditProductCategoryRelation(rels);
            var res = await _productRepository.CreateAsync(newProduct);
            if (res.Success)
                return new OperationResult(true);


            if (command.ImageFile != null)
            {
                _fileService.DeleteImage(FileDirectories.ProductImageDirectory);
                _fileService.DeleteImage(FileDirectories.ProductImageDirectory100);
                _fileService.DeleteImage(FileDirectories.ProductImageDirectory500);

            }
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> EditAsync(EditProductCommandModel command)
        {
            var product = await _productRepository.GetByIdAsync(command.Id);
            if (product == null)
                return new OperationResult(false, "محصول مورد نظر یافت نشد");

            if (await _productRepository.ExistByAsync(t => t.Title == command.Title && t.Id != command.Id))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage);


            if (await _productRepository.ExistByAsync(s => s.Slug == command.Slug && s.Id != command.Id))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage);

            if (command.SelectedCategory?.Count < 1)
                return new OperationResult(false, "لطفا حداقل یک دسته بندی برای محصول انتخاب کنید");

            if (command.Weight < 1)
                return new OperationResult(false, "وزن محصول نمیتواند کمتر از 1 گرم باشد");
            var OldimageName = command.ImageName;
            var NewImageName = command.ImageName;
            if (command.ImageFile != null)
            {
                if (!command.ImageFile.IsImage())
                    return new OperationResult(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

                NewImageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.ProductImageFolder);
                if (string.IsNullOrEmpty(NewImageName))
                    return new OperationResult(false, "خطا در بازگذاری تصویر", nameof(command.ImageFile));

                _fileService.ResizeImage(NewImageName, FileDirectories.ProductImageFolder, 100);
                _fileService.ResizeImage(NewImageName, FileDirectories.ProductImageFolder, 500);



            }
            product.Edit(command.Title, NewImageName, command.ImageAlt.Trim().ToLower(), command.ShortDescription, command.ShortDescription, command.Weight, command.Slug);
            var rels = new List<Product_Category_Rel>();
            foreach (var item in command.SelectedCategory)
            {
                rels.Add(new Product_Category_Rel(item));
            }

            product.EditProductCategoryRelation(rels);
            if (await _productRepository.SaveAsync())
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

        public async Task<EditProductCommandModel> GetForEditAsync(int productId)
        {
            var prodouct = await _productRepository.GetForEditAsync(productId);
            prodouct.Categories = await _productCategoryQueries.GetCategoriesForAddProduct();
            return prodouct;

        }
    }
}
