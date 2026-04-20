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

            var newProductCategory = new ProductCategory(command.Title, command.Slug.Trim().ToLower(),command.ParentId);

            var res = await _productCategoryRepository.CreateAsync(newProductCategory);
            if (res.Success)
                return new OperationResult(true);

            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> EditAsync(EditProductCategoryCommandModel command)
        {

            var productCategory = await _productCategoryRepository.GetByIdAsync(command.Id);
            if (productCategory == null)
                return new OperationResult(false, "دسته بندی مورد نظر یافت نشد");

            if (await _productCategoryRepository.ExistByAsync(t => t.Title == command.Title && t.Id != command.Id))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage);


            if (await _productCategoryRepository.ExistByAsync(s => s.Slug == command.Slug && s.Id != command.Id))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage);

           
         
            productCategory.Edit(command.Title, command.Slug, command.ParentId);
            if (await _productCategoryRepository.SaveAsync())
                return new OperationResult(true);
            
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<EditProductCategoryCommandModel> GetForEditAsync(int productCategoryId)
        {
            return await _productCategoryRepository.GetForEditAsync(productCategoryId); 
        }
    }
}
