using PostModule.Application.Contract.UserPostApplication.Command;
using PostModule.Domain.UserPostAgg;
using Shared;
using Shared.Application;
using Shared.Application.Service;
using Shared.Application.Validations;

namespace PostModule.Application.Services
{
    internal class PackageService : IPackageCommandService
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IFileService _fileService;
        public PackageService(IPackageRepository packageRepository,IFileService fileService)
        {
            _packageRepository = packageRepository;
            _fileService = fileService;
        }

        public async Task<bool> ActivationChange(int id)
        {
            var package = await _packageRepository.GetByIdAsync(id);
            package.ActivationChange();
            return await _packageRepository.SaveAsync();
        }

        public async Task<OperationResult> Create(CreatePackage command)
        {
            if (await _packageRepository.ExistByAsync(p => p.Title.Trim() == command.Title.Trim()))
                return new(false, ValidationMessages.DuplicatedMessage, nameof(command.Title));

            if (command.ImageFile == null || !command.ImageFile.IsImage())
                return new(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

            string imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.PackageImageFolder);
            if (imageName == "")
                return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));

            _fileService.ResizeImage(imageName, FileDirectories.PackageImageFolder, 400);
            _fileService.ResizeImage(imageName, FileDirectories.PackageImageFolder, 100);


            Package package = new(command.Title, command.Description, command.Count, command.Price,imageName,command.ImageAlt);
            var result= await _packageRepository.CreateAsync(package);
            if (result.Success)
            {
                return new(true);
            }
            _fileService.DeleteImage($"{FileDirectories.PackageImageDirectory}{imageName}");
            _fileService.DeleteImage($"{FileDirectories.PackageImageDirectory400}{imageName}");
            _fileService.DeleteImage($"{FileDirectories.PackageImageDirectory100}{imageName}");
            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));
        }

        public async Task<OperationResult> Edit(EditPackage command)
        {
            var package = await _packageRepository.GetByIdAsync(command.Id);
            if (await _packageRepository.ExistByAsync(p => p.Title.Trim() == command.Title.Trim() && p.Id != package.Id))
                return new(false, ValidationMessages.DuplicatedMessage, nameof(command.Title));
            string imageName = package.ImageName;
            string oldImageName = package.ImageName;
            if (command.ImageFile != null)
            {
                imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.PackageImageFolder);
                if (imageName == "")
                    return new(false, ValidationMessages.SystemErrorMessage, "Title");
                _fileService.ResizeImage(imageName, FileDirectories.PackageImageFolder, 400);
                _fileService.ResizeImage(imageName, FileDirectories.PackageImageFolder, 100);
            }

            package.Edit(command.Title, command.Description, command.Count, command.Price,imageName,command.ImageAlt);
            if (await _packageRepository.SaveAsync())
            {
                if (command.ImageFile != null)
                {
                    _fileService.DeleteImage($"{FileDirectories.PackageImageDirectory}{oldImageName}");
                    _fileService.DeleteImage($"{FileDirectories.PackageImageDirectory400}{oldImageName}");
                    _fileService.DeleteImage($"{FileDirectories.PackageImageDirectory100}{oldImageName}");
                }
                return new(true);
            }
            if (command.ImageFile != null)
            {
                _fileService.DeleteImage($"{FileDirectories.PackageImageDirectory}{imageName}");
                _fileService.DeleteImage($"{FileDirectories.PackageImageDirectory400}{imageName}");
                _fileService.DeleteImage($"{FileDirectories.PackageImageDirectory100}{imageName}");
            }
            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));
        }

        public async Task<EditPackage> GetForEdit(int id) =>
           await _packageRepository.GetForEdit(id);
    }
}
