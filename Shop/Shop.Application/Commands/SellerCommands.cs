using Shared;
using Shared.Application;
using Shared.Application.Service;
using Shared.Application.Validations;
using Shared.Domain.Enums;
using Shop.Application.Contract.Seller.Command;
using Shop.Domain.ProductAgg;
using Shop.Domain.SellerAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Commands
{
    internal class SellerCommands : ISellerCommands
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IFileService _fileService;

        public SellerCommands(ISellerRepository sellerRepository, IFileService fileService)
        {
            _sellerRepository = sellerRepository;
            _fileService = fileService;
        }

        public async Task<OperationResult> ChangeSellerStatus(int UserId, SellerStatus sellerStatus)
        {
           var seller= await _sellerRepository.GetByIdAsync(UserId);
            seller.ChangeStatus(sellerStatus);
            if(await _sellerRepository.SaveAsync())
                return new OperationResult(true);   
            return new OperationResult(false , ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> EditRequestForSales(int UserId, EditRequestForSelasCommandModel command)
        {
            var seller = await _sellerRepository.GetByIdAsync(UserId);
            if (seller == null)
                return new OperationResult(false, "فروشنده ای با شناسه ارسال شده یافت نشد");
            if (await _sellerRepository.ExistByAsync(t => t.Title.Trim().ToLower() == command.Title.Trim().ToLower() && t.Id != command.Id))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage);

            var imageName = command.ImageName;
            var oldImageName = command.ImageName;
            if (command.ImageFile != null)
            {
                if (!command.ImageFile.IsImage())
                    return new OperationResult(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

                imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.ProductImageFolder);
                if (string.IsNullOrEmpty(imageName))
                    return new OperationResult(false, "خطا در بازگذاری تصویر", nameof(command.ImageFile));

                _fileService.ResizeImage(imageName, FileDirectories.SellerImageFolder, 100);
                _fileService.ResizeImage(imageName, FileDirectories.SellerImageFolder, 500);
            }

            seller.Edit(command.Title, command.StateId, command.CityId, command.Address
                , command.GoogleMapUrl, imageName, command.ImageAlt, command.Instagram,
                command.Telegram, command.WhatsApp, command.Phone1, command.Phone2, command.Email);
            if (await _sellerRepository.SaveAsync())
            {
                _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory}{oldImageName}");
                _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory100}{oldImageName}");
                _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory500}{oldImageName}");

                return new OperationResult(true);
            }


            if (command.ImageFile != null)
            {
                _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory}{imageName}");
                _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory100}{imageName}");
                _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory500}{imageName}");
            }
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> RequestForSales(int UserId, RequestForSelasCommandModel command)
        {
            if (await _sellerRepository.ExistByAsync(t => t.Title.Trim().ToLower() == command.Title.Trim().ToLower()))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage);

            if (command.ImageFile == null || !command.ImageFile.IsImage())
                return new OperationResult(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

            var imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.SellerImageFolder);
            if (string.IsNullOrEmpty(imageName))
                return new OperationResult(false, "خطا در بازگذاری تصویر", nameof(command.ImageFile));

            _fileService.ResizeImage(imageName, FileDirectories.SellerImageFolder, 100);
            _fileService.ResizeImage(imageName, FileDirectories.SellerImageFolder, 500);

            if (command.LicenseImage == null || !command.LicenseImage.IsImage())
                return new OperationResult(false, ValidationMessages.ImageErrorMessage, nameof(command.LicenseImage));

            var licenseImage = await _fileService.UploadImage(command.LicenseImage, FileDirectories.SellerImageFolder);
            if (string.IsNullOrEmpty(licenseImage))
                return new OperationResult(false, "خطا در بارگذاری تصویر", nameof(command.ImageFile));

            _fileService.ResizeImage(licenseImage, FileDirectories.SellerImageFolder, 100);
            _fileService.ResizeImage(licenseImage, FileDirectories.SellerImageFolder, 500);

            var seller = new Seller(UserId, command.Title, command.StateId, command.CityId, command.Address
                , command.GoogleMapUrl, imageName, licenseImage, command.ImageAlt, command.Instagram,
                command.Telegram, command.WhatsApp, command.Phone1, command.Phone2, command.Email);
            var res = await _sellerRepository.CreateAsync(seller);
            if (res.Success)
                return new OperationResult(true);

            if (command.ImageFile != null)
            {
                _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory}{imageName}");
                _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory100}{imageName}");
                _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory500}{imageName}");
            }
            if (command.LicenseImage != null)
            {
                _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory}{licenseImage}");
                _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory100}{licenseImage}");
                _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory500}{licenseImage}");
            }

            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }


    }
}
