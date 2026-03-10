using Shared.Application;
using Shared.Application.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.RoleService.Command;
using Users.Domain.User.Agg;
using Users.Domain.User.Agg.IRepository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Users.Application.Services
{
    internal class RoleService : IRoleCommandService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public RoleService(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task<OperationResult> CreateAsync(CreateRoleCommand command, List<int> permissions)
        {
            if (await _roleRepository.ExistByAsync(t => t.Title == command.Title.Trim().ToLower()))
                return new(false, ValidationMessages.DuplicatedMessage,nameof(command.Title));

            var role = new Role(command.Title.Trim().ToLower());
            if (permissions.Count > 0)
            {
                foreach (var permission in permissions)
                {
                    role.AddPermission(permission, role.Id);
                }

            }
            var result = await _roleRepository.CreateAsync(role);

            if (result.Success)
                return new(true);

            return new(false, ValidationMessages.SystemErrorMessage, "Role");
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            // 1 is SiteManager 2  is User
            if (id == 1 || id == 2)
                return new(false, "امکان حذف این نقش وجود ندارد", "Role");
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) return new(false, ValidationMessages.SystemErrorMessage);

            var result = await _roleRepository.DeleteAsync(role);
            if (result.Success) return new(true);

            return new(false, result.Message, result.ModelName);
        }

        public async Task<OperationResult> EditAsync(EditRoleCommand command, List<int> permissions)
        {
            return await _roleRepository.EditAsync(command, permissions);

        }

    }
}
