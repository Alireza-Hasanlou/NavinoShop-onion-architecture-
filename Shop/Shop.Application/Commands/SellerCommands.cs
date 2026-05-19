using Shared;
using Shared.Application;
using Shared.Application.Service;
using Shared.Application.Validations;
using Shared.Domain.Enums;
using Shop.Application.Contract.Seller.Command;
using Shop.Domain.SellerAgg;
using Shop.Domain.SellerChangeRequestsAgg;


namespace Shop.Application.Commands
{
    internal class SellerCommands : ISellerCommands
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly ISellerChangeRequestsRepository _SellerchangeRequestsRepository;
        private readonly IFileService _fileService;

        public SellerCommands(ISellerRepository sellerRepository, ISellerChangeRequestsRepository changeRequestsRepository, IFileService fileService)
        {
            _sellerRepository = sellerRepository;
            _SellerchangeRequestsRepository = changeRequestsRepository;
            _fileService = fileService;
        }

        public async Task<OperationResult> ChangeSellerStatus(int Id, SellerStatus sellerStatus, string? whyRejected)
        {
            var seller = await _sellerRepository.GetByIdAsync(Id);
            seller.ChangeStatus(sellerStatus, whyRejected);
            if (await _sellerRepository.SaveAsync())
                return new OperationResult(true);
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> EditRequestForSales(EditRequestForSelasCommandModel command)
        {
            var seller = await _sellerRepository.GetByIdAsync(command.Id);
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

                imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.SellerImageFolder);
                if (string.IsNullOrEmpty(imageName))
                    return new OperationResult(false, "خطا در بازگذاری تصویر", nameof(command.ImageFile));

                _fileService.ResizeImage(imageName, FileDirectories.SellerImageFolder, 100);
                _fileService.ResizeImage(imageName, FileDirectories.SellerImageFolder, 500);
            }
            var CoverimageName = command.CoverImageName;
            var oldCoverimageName = command.CoverImageName;
            if (command.CoverImage != null)
            {
                if (!command.CoverImage.IsImage())
                    return new OperationResult(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

                CoverimageName = await _fileService.UploadImage(command.CoverImage, FileDirectories.SellerImageFolder);
                if (string.IsNullOrEmpty(imageName))
                    return new OperationResult(false, "خطا در بازگذاری تصویر", nameof(command.ImageFile));
            }
            var licenseImage = command.LicenseImageName;
            var oldlicenseImage = command.LicenseImageName;
            if (command.LicenseImage != null)
            {
                if (!command.LicenseImage.IsImage())
                    return new OperationResult(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

                licenseImage = await _fileService.UploadImage(command.LicenseImage, FileDirectories.SellerImageFolder);
                if (string.IsNullOrEmpty(imageName))
                    return new OperationResult(false, "خطا در بازگذاری تصویر", nameof(command.ImageFile));

                _fileService.ResizeImage(licenseImage, FileDirectories.SellerImageFolder, 100);
                _fileService.ResizeImage(licenseImage, FileDirectories.SellerImageFolder, 500);
            }

            seller.Edit(command.Title, command.StateId, command.CityId, command.Address
                , command.GoogleMapUrl, imageName, licenseImage, command.ImageAlt, command.Instagram,
                command.Telegram, command.WhatsApp, command.Phone1, command.Phone2, command.Email, CoverimageName);
            seller.ChangeStatus(SellerStatus.درخواست_ارسال_شده, "");
            if (await _sellerRepository.SaveAsync())
            {
                if (command.ImageFile != null)
                {
                    DeleteSellerImage(oldImageName);
                }
                if (command.LicenseImage != null)
                {
                    DeleteSellerImage(oldlicenseImage);
                }
                if (command.CoverImage != null)
                {
                    _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory}{oldCoverimageName}");
                }
                return new OperationResult(true);
            }
            else
            {
                if (command.ImageFile != null)
                {
                    DeleteSellerImage(imageName);
                }
                if (command.LicenseImage != null)
                {
                    DeleteSellerImage(licenseImage);
                }
                if (command.CoverImage != null)
                {
                    _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory}{CoverimageName}");
                }
            }
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> SendSellerChangeRequests(EditSellerQueryModel command)
        {
            var seller = await _sellerRepository.GetByIdAsync(command.SellerId);
            if (seller == null)
                return new OperationResult(false, "فروشنده ای با شناسه ارسال شده یافت نشد");
            if (await _SellerchangeRequestsRepository.ExistByAsync(x => x.SellerId == command.SellerId && x.status ==SellerStatus.درخواست_ارسال_شده))
                return new OperationResult(false, " شما در حال حاضر یک درخواست درحال بررسی در دارید و درخواست جدیدی نمیتوانید ثبت کنید");
            if (await _sellerRepository.ExistByAsync(t => t.Title.Trim().ToLower() == command.Title.Trim().ToLower() && t.Id != command.SellerId))
                return new OperationResult(false, ValidationMessages.DuplicatedMessage);

            var imageName = command.AvatarImageName;
            if (command.ImageFile != null)
            {
                if (!command.ImageFile.IsImage())
                    return new OperationResult(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

                imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.SellerImageFolder);
                if (string.IsNullOrEmpty(imageName))
                    return new OperationResult(false, "خطا در بازگذاری تصویر", nameof(command.ImageFile));

                _fileService.ResizeImage(imageName, FileDirectories.SellerImageFolder, 100);
                _fileService.ResizeImage(imageName, FileDirectories.SellerImageFolder, 500);
            }
            var CoverimageName = command.CoverImageName;
            if (command.CoverImage != null)
            {
                if (!command.CoverImage.IsImage())
                    return new OperationResult(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

                CoverimageName = await _fileService.UploadImage(command.CoverImage, FileDirectories.SellerImageFolder);
                if (string.IsNullOrEmpty(imageName))
                    return new OperationResult(false, "خطا در بازگذاری تصویر", nameof(command.ImageFile));
            }

            var SellerChangeRequests = new SellerChangeRequest(seller.Id, imageName, CoverimageName, command.Title, command.Address
                , command.GoogleMapUrl, command.WhatsApp, command.Telegram, command.Instagram, command.Phone1, command.Phone2, command.Email,command.Description);
            var res = await _SellerchangeRequestsRepository.CreateAsync(SellerChangeRequests);
            if (res.Success)
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<EditRequestForSelasCommandModel> GetForEditRequestForSales(int Id)
        {
            var request = await _sellerRepository.GetByIdAsync(Id);
            if (request == null)
                return null;

            return new EditRequestForSelasCommandModel
            {
                Id = request.Id,
                Address = request.Address,
                CityId = request.CityId,
                Email = request.Email,
                GoogleMapUrl = request.MapUrl,
                ImageName = request.ImageName,
                Instagram = request.Instagram,
                Phone1 = request.Phone1,
                Phone2 = request.Phone2,
                StateId = request.StateId,
                Telegram = request.Telegram,
                Title = request.Title,
                WhatsApp = request.Whatsup,
                CoverImageName = request.CoverImage,
            };
        }

        public async Task<EditSellerQueryModel> GetForEditSellerAsync(int Id)
        {
            var request = await _sellerRepository.GetByIdAsync(Id);
            if (request == null)
                return null;

            return new EditSellerQueryModel
            {
                SellerId = request.Id,
                Address = request.Address,
                Email = request.Email,
                GoogleMapUrl = request.MapUrl,
                AvatarImageName = request.ImageName,
                Instagram = request.Instagram,
                Phone1 = request.Phone1,
                Phone2 = request.Phone2,
                Telegram = request.Telegram,
                Title = request.Title,
                WhatsApp = request.Whatsup,
                CoverImageName = request.CoverImage,
            };
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
                DeleteSellerImage(imageName);
            }
            if (command.LicenseImage != null)
            {
                DeleteSellerImage(licenseImage);
            }

            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }
        private void DeleteSellerImage(string imageName)
        {

            _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory}{imageName}");
            _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory100}{imageName}");
            _fileService.DeleteImage($"{FileDirectories.SellerImageDirectory500}{imageName}");
        }

        public async Task<OperationResult> AcceptRequestChange(int id)
        {
            var request = await _SellerchangeRequestsRepository.GetByIdAsync(id);
            if (request == null)
                return new(false, "خطا در انجام عملیات");

            var seller = await _sellerRepository.GetByIdAsync(request.SellerId);
            seller.Edit(request.Title, seller.StateId, seller.CityId,
                request.Address, request.GoogleMapUrl, request.Avatar,
                seller.LicenseImage, seller.ImageAlt, request.Instagram,
                request.Telegram, request.WhatsApp, request.Phone1,
                request.Phone2, request.Email, request.CoverImage);

            if (await _sellerRepository.SaveAsync())
            {
                request.ChangeRequestStatus(SellerStatus.درخواست_تایید_شده,"");
                await _SellerchangeRequestsRepository.SaveAsync();
                //TODO Send Message
                return new(true);
            }

            return new(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> RejectRequestChange(int id ,string why)
        {
            var request = await _SellerchangeRequestsRepository.GetByIdAsync(id);
            if (request == null)
                return new(false, "خطا در انجام عملیات");

            request.ChangeRequestStatus(SellerStatus.درخواست_تایید_نشده,why);
            if (await _SellerchangeRequestsRepository.SaveAsync())
            {
                
                return new(true);
                //TODO Send Message
            }
            return new(false, ValidationMessages.SystemErrorMessage);
        }


    }
}
