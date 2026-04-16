using Shared;
using Shared.Application;
using Shared.Application.Auth;
using Shared.Application.Security;
using Shared.Application.Service;
using Shared.Application.Validations;
using Users.Application.Contract.RoleService.Command;
using Users.Application.Contract.RoleService.Query;
using Users.Application.Contract.UserService.Command;
using Users.Application.Contract.UserService.Query;
using Users.Domain.User.Agg;
using Users.Domain.User.Agg.IRepository;


namespace Users.Application.Services
{
    internal class UserService : IUserCommandService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;
        private readonly IAuthService _authService;
        private readonly IRoleCommandService _roleCommandService;

        public UserService(IUserRepository userRepository, IFileService fileService,
            IAuthService authService, IRoleCommandService roleCommandService)
        {
            _userRepository = userRepository;
            _fileService = fileService;
            _authService = authService;
            _roleCommandService = roleCommandService;
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

            if (!string.IsNullOrEmpty(command.Email))
            {
                if (await _userRepository.ExistByAsync(e => e.Email.ToLower() == command.Email.ToLower()))
                    return new(false, ValidationMessages.DuplicatedMessage, "Email");
            }
            else { command.Email = ""; }


            var user = new User(command.FullName, command.Mobile.Trim(), command.Email.Trim().ToLower(),
                   command.Password, "DefaultAvatar.png", true, false, command.UserGender);

            var result = await _userRepository.CreateAsync(user);
            if (result.Success)
                return new(true);
            return new(false, ValidationMessages.SystemErrorMessage, "User");


        }

        public async Task<OperationResult> DeleteChangeAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            user.DeleteChange();
            return new(await _userRepository.SaveAsync());
        }

        public async Task<OperationResult> EditByAdminAsync(EditUserByAdminDto command, List<int> roles)
        {
            var user = await _userRepository.GetForEditByAdmin(command.Id);
            if (user == null)
                return new(false, ValidationMessages.UserNotFound, "User");

            if (await _userRepository.ExistByAsync(m => m.Mobile.Trim() == command.Mobile.Trim() && m.Id != user.Id))
                return new(false, ValidationMessages.DuplicatedMessage, "Mobile");

            if (!string.IsNullOrEmpty(command.Email))
            {
                if (await _userRepository.ExistByAsync(e => e.Email.ToLower() == command.Email.ToLower() && e.Id != user.Id))
                    return new(false, ValidationMessages.DuplicatedMessage, "Email");
            }
            else { command.Email = ""; }


            string imageName = user.Avatar;
            string oldImageName = user.Avatar;

            if (command.AvatarFile != null)
            {
                if (!FileSecurity.IsImage(command.AvatarFile))
                    return new(false, ValidationMessages.ImageErrorMessage, "Avatar");

                imageName = await _fileService.UploadImage(command.AvatarFile, FileDirectories.UserImageFolder);
                if (string.IsNullOrEmpty(imageName))
                    return new(false, ValidationMessages.SystemErrorMessage, "Avatar");

                DeleteUserImages(oldImageName);
                _fileService.ResizeImage(imageName, FileDirectories.UserImageFolder, 100);
            }

            string hashPassword = user.Password;
            if (!string.IsNullOrEmpty(command.Password))
                hashPassword = Sha256Hasher.Hash(command.Password);
            
            user.Edit(command.FullName, command.Mobile.Trim(), command.Email.Trim().ToLower(), hashPassword, imageName, command.UserGender,roles);
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

            string imageName = user.Avatar;
            string oldimageName = user.Avatar;
            if (command.AvatarFile != null)
            {
                if (!FileSecurity.IsImage(command.AvatarFile))
                    return new(false, ValidationMessages.ImageErrorMessage, "image");

                imageName = await _fileService.UploadImage(command.AvatarFile, FileDirectories.UserImageFolder);
                if (imageName == "")
                    return new(false, ValidationMessages.SystemErrorMessage, "Avatar");
                DeleteUserImages(oldimageName);
                _fileService.ResizeImage(imageName, FileDirectories.UserImageFolder, 100);
            }
            user.Edit(command.FullName, command.Mobile.Trim(), command.Email.Trim().ToLower(), user.Password, imageName, command.UserGender,new List<int>());
            var resut = await _userRepository.SaveAsync();
            if (resut)
                return new(true);


            if (command.AvatarFile != null && oldimageName != "Default.png")
            {
                DeleteUserImages(imageName);
            }

            return new(false, ValidationMessages.SystemErrorMessage, "User");

        }

        public async Task<EditUserByUserDto> GetForEditByUserAsync(int userId)
        {
            return await _userRepository.GetForEditByUserAsync(userId);
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
                Email = user.Email,
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
                    //3 Is customer
                    newUser.AddRole(3);
                    // send sms active code
                    //TODO
                    return new(true, "", "");
                }
                else
                {
                    return new OperationResult(false, "شما قبلا در سایت ثبت نام کردید");
                }

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
