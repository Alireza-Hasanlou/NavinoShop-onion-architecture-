using Shared;
using Shared.Application;
using Shared.Application.Service;
using Shared.Application.Validations;
using Shop.Application.Contract.ProductGallery.Command;
using Shop.Domain.ProductGalleryAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Commands
{
    internal class ProductGalleryCommands : IProductGalleryCommands
    {
        private readonly IProductGalleryRepository _productGalleryRepository;
        private readonly IFileService _fileService;

        public ProductGalleryCommands(IProductGalleryRepository productGalleryRepository, IFileService fileService)
        {
            _productGalleryRepository = productGalleryRepository;
            _fileService = fileService;
        }

        public async Task<OperationResult> CreateAsync(CreateProductGalleryCommandModel command)
        {
            if (command.ImageFile == null || !command.ImageFile.IsImage())
                return new OperationResult(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

            var imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.ProductGalleryImageFolder);
            if (!string.IsNullOrEmpty(imageName))
            {
                _fileService.ResizeImage(imageName, FileDirectories.ProductGalleryImageFolder, 100);
                _fileService.ResizeImage(imageName, FileDirectories.ProductGalleryImageFolder, 500);

            }
            var res = await _productGalleryRepository.CreateAsync(new ProductGallery(command.ProductId, imageName, command.ImageAlt));
            if (res.Success)
                return new OperationResult(true);

            if (command.ImageFile != null)
            {
                _fileService.DeleteImage(FileDirectories.ProductGalleryImageDirectory);
                _fileService.DeleteImage(FileDirectories.ProductGalleryImageDirectory100);
                _fileService.DeleteImage(FileDirectories.ProductGalleryImageDirectory500);
            }
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> DeleteAsync(int Id)
        {
            var productGallery = await _productGalleryRepository.GetByIdAsync(Id);
            if (productGallery == null)
                return new OperationResult(false, "تصویری با شناسه ارسال شده یافت نشد");
            var res = await _productGalleryRepository.DeleteAsync(productGallery);
            if (res.Success)
                return new OperationResult(true);
            return new OperationResult(false,ValidationMessages.SystemErrorMessage);
        }
    }
}
