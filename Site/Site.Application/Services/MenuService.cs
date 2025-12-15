using Shared.Application;
using Site.Domain.MenuAgg;
using Shared;
using Shared.Application.Service;
using Site.Application.Contract.MenuService.Command;
using System.Threading.Tasks;
using Shared.Application.Validations;
using Shared.Domain.Enums;

namespace Site.Application.Services
{
    internal class MenuService : IMenuCommandService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IFileService _fileService;

        public MenuService(IMenuRepository menuRepository, IFileService fileService)
        {
            _menuRepository = menuRepository;
            _fileService = fileService;
        }

        public async Task<OperationResult> ActivationChangeAsync(int id)
        {
            var menu = await _menuRepository.GetByIdAsync(id);
            menu.ActivationChange();
            if (await _menuRepository.SaveAsync())
                return new(true);
            return new(false);
        }

        public async Task<OperationResult> CreateAsync(CreateMenuCommandModel command)
        {
            if(command.Status== MenuStatus.زیرمنوی_سردسته
                || command.Status == MenuStatus.دسته_های_گروه_مجصولات
                || command.Status == MenuStatus.زیردسته_های_گروه_محصولات
                || command.Status == MenuStatus.زیرمنوی_سردسته
                || command.Status == MenuStatus.منوی_فوتر)
                return new(false, "در این بخض فقط سردسته منو ها را میتوانید ایجاد کنید", nameof(command.Status));

            if(await _menuRepository.ExistMainMenu(command.Status))
                return new(false, "منوی گروه محصولات از قبل موجود است", nameof(command.Status));

            if (command.Status == MenuStatus.دسته_های_گروه_مجصولات)
            {
                if (command.ImageFile == null || !command.ImageFile.IsImage())
                    return new(false, $"{MenuStatus.دسته_های_گروه_مجصولات.ToString().Replace("_", " ")} نیاز به یک تصویر دارد", nameof(command.ImageFile));
                else if (string.IsNullOrEmpty(command.ImageAlt))
                    return new(false, ValidationMessages.RequiredMessage, nameof(command.ImageAlt));
            }
            else
            {
                if (command.ImageFile != null)
                    return new(false, $"{command.Status.ToString().Replace("_", " ")} نیاز به تصویر ندارد", nameof(command.ImageFile));
                if (!string.IsNullOrEmpty(command.ImageAlt))
                    return new(false, $"{command.Status.ToString().Replace("_", " ")} نیاز به Alt تصویر ندارد", nameof(command.ImageAlt));
            }
            string imageName = "";
            if (command.ImageFile != null)
            {
                imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.MenuImageFolder);
                if (imageName == "")
                    return new(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

                _fileService.ResizeImage(imageName, FileDirectories.MenuImageFolder, 100);
            }

            Menu menu = new(command.Number, command.Title, command.Url, command.Status, imageName, command.ImageAlt, null);
            var result = await _menuRepository.CreateAsync(menu);
            if (result.Success)
                return new(true);
            if (command.ImageFile != null)
            {
                _fileService.DeleteImage($"{FileDirectories.MenuImageDirectory}{imageName}");
                _fileService.DeleteImage($"{FileDirectories.MenuImageDirectory100}{imageName}");
            }
            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.ImageAlt));
        }

        public async Task<OperationResult> CreateSubAsync(CreateSubMenuCommandModel command)
        {

            MenuStatus status = MenuStatus.منوی_اصلی;
            switch (command.ParentStatus)
            {
                case MenuStatus.منوی_گروه_محصولات:
                    status = MenuStatus.دسته_های_گروه_مجصولات;
                    break;

                case MenuStatus.دسته_های_گروه_مجصولات:
                    status = MenuStatus.زیردسته_های_گروه_محصولات;
                    break;
                case MenuStatus.زیردسته_های_گروه_محصولات:
                    status = MenuStatus.محصولات_در_زیردسته_گروه_محصولات;
                    break;
                case MenuStatus.منوی_اصلی_با_زیر_منو:
                    status = MenuStatus.زیرمنوی_سردسته;
                    break;
                case MenuStatus.تیتر_منوی_فوتر:
                    status = MenuStatus.منوی_فوتر;
                    break;
                default:
                    return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));
            }
            Menu menu = new(command.Number, command.Title, command.Url, status, "", command.ImageAlt, command.ParentId);
            var result = await _menuRepository.CreateAsync(menu);
            if (result.Success)
                return new(true);

            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));
        }

        public async Task<OperationResult> EditAsync(EditMenuCommandModel command)
        {
            var menu = await _menuRepository.GetByIdAsync(command.Id);
            string imageName = command.ImageName;
            string oldImageName = command.ImageName;
            if (command.ImageFile != null && string.IsNullOrEmpty(command.ImageAlt))
                return new(false, ValidationMessages.RequiredMessage, nameof(command.ImageAlt));
            if (command.ImageFile != null && !command.ImageFile.IsImage())
                return new(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));
            if (command.ImageFile != null)
            {
                imageName = await _fileService.UploadImage(command.ImageFile, FileDirectories.MenuImageFolder);
                if (imageName == ""&& command.Status==MenuStatus.زیردسته_های_گروه_محصولات)
                    return new(false, ValidationMessages.ImageErrorMessage, nameof(command.ImageFile));

                _fileService.ResizeImage(imageName, FileDirectories.MenuImageFolder, 100);
            }
            menu.Edit(command.Number, command.Title, command.Url, imageName, command.ImageAlt);
            if (await _menuRepository.SaveAsync())
            {
                if (command.ImageFile != null && !string.IsNullOrEmpty(oldImageName))
                {
                    _fileService.DeleteImage($"{FileDirectories.MenuImageDirectory}{oldImageName}");
                    _fileService.DeleteImage($"{FileDirectories.MenuImageDirectory100}{oldImageName}");
                }
                return new(true);
            }
            if (command.ImageFile != null)
            {
                _fileService.DeleteImage($"{FileDirectories.MenuImageDirectory}{imageName}");
                _fileService.DeleteImage($"{FileDirectories.MenuImageDirectory100}{imageName}");
            }
            return new(false, ValidationMessages.SystemErrorMessage, nameof(command.ImageAlt));
        }

        public async Task<CreateSubMenuCommandModel> GetForCreateAsync(int parentId)
        {
            var parent = await _menuRepository.GetByIdAsync(parentId);
            return new()
            {
                ImageAlt = "",
                ImageFile = null,
                ParentId = parent.Id,
                ParentStatus = parent.Status,
                ParentTitle = $"افزودن زیر منو برای {parent.Title}"

            };
        }

        public async Task<EditMenuCommandModel> GetForEditAsync(int id) =>
          await _menuRepository.GetForEdit(id);
    }
}
