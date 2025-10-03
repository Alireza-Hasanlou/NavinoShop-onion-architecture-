using Shared;
using Shared.Application;
using Shared.Application.Auth;
using Shared.Application.Security;
using Shared.Application.Service;
using Shared.Application.Validations;
using Users.Application.Contract.UserService.Command;
using Users.Domain.User.Agg;
using Users.Domain.User.Agg.IRepository;


namespace Users.Application.Services
{
    internal class UserService : IUserCommandService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;
        private readonly IAuthService _authService;

        public UserService(IUserRepository userRepository, IFileService fileService, IAuthService authService)
        {
            _userRepository = userRepository;
            _fileService = fileService;
            _authService = authService;
        }

        public async Task<OperationResult> ActivationChangeAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            user.ActivationChange();
            return new(await _userRepository.SaveAsync());
        }

        public async Task<OperationResult> ChangePasswordAsync(ChangeUserPasswordCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            var HashOldPassword = Sha256Hasher.Hash(command.OldPassword);

            if (user.Password != HashOldPassword)
                return new(false, ValidationMessages.OldPasswordErrorMessage, "OldPassword");

            var HashNewPassword = Sha256Hasher.Hash(command.NewPassword);

            user.ChangePassword(HashNewPassword);
            return new(await _userRepository.SaveAsync());
        }

        public async Task<OperationResult> CreateAsync(CreateUserCommand command)
        {

            if (await _userRepository.ExistByAsync(m => m.Mobile.Trim() == command.Mobile.Trim()))
                return new(false, ValidationMessages.DuplicatedMessage, "Mobile");

            if (!string.IsNullOrEmpty(command.Email) && await _userRepository.ExistByAsync(e => e.Email.ToLower() == command.Email.ToLower()))
                return new(false, ValidationMessages.DuplicatedMessage, "Email");

            string imageName = FileDirectories.UserDefaultAvatar;
            if (command.AvatarFile != null)
            {
                if (!FileSecurity.IsImage(command.AvatarFile))
                    return new(false, ValidationMessages.ImageErrorMessage, "image");

                imageName = await _fileService.UploadImage(command.AvatarFile, FileDirectories.UserImageDirectory);
                if (imageName == "")
                    return new(false, ValidationMessages.SystemErrorMessage, "Avatar");
                _fileService.ResizeImage(imageName, FileDirectories.UserImageFolder, 100);
            }



            //var HashPassword = Sha256Hasher.Hash(command.Password);
            var Key = GenerateRandomCode.GenerateUserRegisterCode().ToString();
            var user = new User(command.FullName, command.Mobile.Trim(), command.Email.Trim().ToLower(),
                   Key, imageName, true, false, command.UserGender);

            var result = await _userRepository.CreateAsync(user);
            if (result.Success)
                return new(true);

            if (command.AvatarFile != null)
            {
                try { _fileService.DeleteImage(Path.Combine(FileDirectories.UserImageDirectory, imageName)); } catch { }
                try { _fileService.DeleteImage(Path.Combine(FileDirectories.UserImageDirectory100, imageName)); } catch { }
            }
            return new(false, ValidationMessages.SystemErrorMessage, "User");


        }

        public async Task<OperationResult> DeleteChangeAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            user.DeleteChange();
            return new(await _userRepository.SaveAsync());
        }

        public async Task<OperationResult> EditByAdminAsync(EditUserByAdminCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.Id);
            if (user == null)
                return new(false, ValidationMessages.UserNotFound, "User");

            if (await _userRepository.ExistByAsync(m => m.Mobile.Trim() == command.Mobile.Trim() && m.Id != user.Id))
                return new(false, ValidationMessages.DuplicatedMessage, "Mobile");

            if (!string.IsNullOrEmpty(command.Email) &&
                await _userRepository.ExistByAsync(e => e.Email.ToLower() == command.Email.ToLower() && e.Id != user.Id))
                return new(false, ValidationMessages.DuplicatedMessage, "Email");

            string imageName = user.Avatar;
            string oldImageName = user.Avatar;

            if (command.AvatarFile != null)
            {
                if (!FileSecurity.IsImage(command.AvatarFile))
                    return new(false, ValidationMessages.ImageErrorMessage, "Avatar");

                imageName = await _fileService.UploadImage(command.AvatarFile, FileDirectories.UserImageDirectory);
                if (string.IsNullOrEmpty(imageName))
                    return new(false, ValidationMessages.SystemErrorMessage, "Avatar");

                DeleteUserImages(oldImageName);
                _fileService.ResizeImage(imageName, FileDirectories.UserImageDirectory100, 100);
            }

            string hashPassword = user.Password;
            if (!string.IsNullOrEmpty(command.Password))
                hashPassword = Sha256Hasher.Hash(command.Password);

            user.Edit(command.FullName, command.Mobile.Trim(), command.Email.Trim().ToLower(), hashPassword, imageName, command.UserGender);

            if (await _userRepository.SaveAsync())
                return new(true);


            if (command.AvatarFile != null && oldImageName != "Default.png")
            {
                DeleteUserImages(imageName);
            }

            return new(false, ValidationMessages.SystemErrorMessage, "User");
        }

        public async Task<OperationResult> EditByUserAsync(EditUserByUserCommand command, int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return new(false, ValidationMessages.UserNotFound, "User");

            if (await _userRepository.ExistByAsync(m => m.Mobile.Trim() == command.Mobile.Trim()))
                return new(false, ValidationMessages.DuplicatedMessage, "Mobile");

            if (!string.IsNullOrEmpty(command.Email) && await _userRepository.ExistByAsync(e => e.Email.ToLower() == command.Email.ToLower()))
                return new(false, ValidationMessages.DuplicatedMessage, "Email");

            string imageName = user.Avatar;
            string oldimageName = user.Avatar;
            if (command.AvatarFile != null)
            {
                if (!FileSecurity.IsImage(command.AvatarFile))
                    return new(false, ValidationMessages.ImageErrorMessage, "image");

                imageName = await _fileService.UploadImage(command.AvatarFile, FileDirectories.UserImageDirectory);
                if (imageName == "")
                    return new(false, ValidationMessages.SystemErrorMessage, "Avatar");
                DeleteUserImages(oldimageName);
                _fileService.ResizeImage(imageName, FileDirectories.UserImageDirectory100, 100);
            }
            user.Edit(command.FullName, command.Mobile.Trim(), command.Email.Trim().ToLower(), user.Password, imageName, command.UserGender);
            var resut = await _userRepository.SaveAsync();
            if (resut)
                return new(true);


            if (command.AvatarFile != null && oldimageName != "Default.png")
            {
                DeleteUserImages(imageName);
            }

            return new(false, ValidationMessages.SystemErrorMessage, "User");

        }

        public async Task<OperationResult> LoginAsync(LoginUserCommand command)
        {
            var user = await _userRepository.GetByMobile(command.Mobile.Trim());
            if (user == null)
                return new(false, ValidationMessages.UserNotFound, nameof(user));
           
            if (user.Password != command.Password.Trim())
                return new(false, ValidationMessages.PasswordLoginError, nameof(command.Password));
            var result = await _authService.LoginAsync(new AuthModel
            {
                Avatar = user.Avatar,
                FullName = string.IsNullOrEmpty(user.FullName) ? user.Mobile : user.FullName,
                Mobile = command.Mobile,
                UserId = user.Id,
            });

            if (result)
                return new(true);

            return new(false, ValidationMessages.SystemErrorMessage, nameof(user));
        }

        public async Task LogoutAsync()
        {
            await _authService.LogoutAsync();

        }

        public async Task<OperationResult> RegisterAsync(RegisterUserCommand command)
        {
            var Key = GenerateRandomCode.GenerateUserRegisterCode().ToString();
            var user = await _userRepository.GetByMobile(command.Mobile.Trim());
            try
            {
                if (user == null)
                {
                    var newUser = User.Register(command.Mobile.Trim(), Key);
                    var res = await _userRepository.CreateAsync(newUser);
                    // send sms active code
                }
                else
                {
                    user.ChangePassword(Key);
                    await _userRepository.SaveAsync();
                }
                return new(true);
            }
            catch (Exception)
            {

                return new(false, ValidationMessages.SystemErrorMessage, "User");
            }
        }
        private void DeleteUserImages(string imageName)
        {
            try { _fileService.DeleteImage(Path.Combine(FileDirectories.UserImageDirectory, imageName)); } catch { }
            try { _fileService.DeleteImage(Path.Combine(FileDirectories.UserImageDirectory100, imageName)); } catch { }
        }

    }
}
