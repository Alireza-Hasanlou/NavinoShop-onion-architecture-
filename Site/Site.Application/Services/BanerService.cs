
using Shared;
using Shared.Application;
using Shared.Application.Service;
using Shared.Application.Validations;
using Site.Application.Contract.BanerService.Command;
using Site.Domain.BanerAgg;



namespace Site.Application.Services
{
    internal class BanerService : IBanerCommandService
    {
        private readonly IBanerRepository _banerRepository;
        private readonly IFileService _fileService;

        public BanerService(IBanerRepository banerRepository, IFileService fileService)
        {
            _banerRepository = banerRepository;
            _fileService = fileService;
        }

        public async Task<OperationResult> ActivationChangeAsync(int id)
        {
            var baner = await _banerRepository.GetByIdAsync(id);
            baner.ActivationChange();
            if (await _banerRepository.SaveAsync())
                return new(true);
            return new(false);
        }

        public async Task<OperationResult> CreateAsync(CreateBanerCommandModel command)
        {
            if (command.ImageFile == null ||  !command.ImageFile.IsImage())
                return new(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

            string imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.BanerImageFolder);
            if (imageName == "")
                return new(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

            _fileService.ResizeImage(imageName, FileDirectories.BanerImageFolder, 100);
            Baner baner = new(imageName, command.ImageAlt, command.Url, command.State);
            var result = await _banerRepository.CreateAsync(baner);
            if(result.Success)
                return new(true);

            _fileService.DeleteImage($"{FileDirectories.BanerImageDirectory}{imageName}");
            _fileService.DeleteImage($"{FileDirectories.BanerImageDirectory100}{imageName}");
            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.ImageAlt));
        }

        public async Task<OperationResult> EditAsync(EditBanerCommandModel command)
        {
            var baner = await _banerRepository.GetByIdAsync(command.Id);
            string imageName = baner.ImageName;
            string oldImageName = baner.ImageName;
            if (command.ImageFile != null)
            {
                if (!command.ImageFile.IsImage()) return new(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));
                imageName =await _fileService.UploadImage(command.ImageFile, FileDirectories.BanerImageFolder);
                if (imageName == "")
                    return new(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));
                _fileService.ResizeImage(imageName, FileDirectories.BanerImageFolder, 100);
            }
                baner.Edit(imageName, command.ImageAlt, command.Url);
                if (await _banerRepository.SaveAsync())
                {
                    if(command.ImageFile != null)
                    {
                        _fileService.DeleteImage($"{FileDirectories.BanerImageDirectory}{oldImageName}");
                        _fileService.DeleteImage($"{FileDirectories.BanerImageDirectory100}{oldImageName}");
                    }
                    return new(true);
                }
                else
                {

                    if (command.ImageFile != null)
                    {
                        _fileService.DeleteImage($"{FileDirectories.BanerImageDirectory}{imageName}");
                        _fileService.DeleteImage($"{FileDirectories.BanerImageDirectory100}{imageName}");
                    }
                    return new(false, ValidationMessages.SystemErrorMessage, nameof(command.ImageAlt));
                }
            
        }

        public async Task<EditBanerCommandModel> GetForEditAsync(int id) =>
           await _banerRepository.GetForEdit(id);
    }
}
