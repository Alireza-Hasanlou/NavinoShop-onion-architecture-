using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.RoleService.Command;
using Users.Domain.User.Agg;
using Users.Domain.User.Agg.IRepository;
using Utility.Shared.Application;

namespace Users.Application.Services
{
    internal class RoleService : IRoleCommandService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<OperationResult> CreateAsync(CreateRoleCommand command, List<int> permissions)
        {
            if (await _roleRepository.ExistByAsync(t => t.Title == command.Title.Trim().ToLower()))
                return new(false);

            var role = new Role(command.Title.Trim().ToLower());
            if (permissions.Count > 0)
            {
                foreach (var permission in permissions)
                {
                    role.AddPermission(permission);
                }

            }
            var result = await _roleRepository.CreateAsync(role);

            if (result.Success)
                return new(true);

            return new(false, ValidationMessages.SystemErrorMessage, "Role");
        }

        public async Task<OperationResult> EditAsync(EditRoleCommand command, List<int> permissions)
        {
            var role = await _roleRepository.GetByIdAsync(command.Id);

            if (await _roleRepository.ExistByAsync(t => t.Title == command.Title.Trim().ToLower() && role.Id != command.Id))
                return new(false, ValidationMessages.DuplicatedMessage, "Role");

            role.Edit(command.Title.Trim().ToLower());

            if (permissions.Count > 0)
            {
                role.ClearPermissions();

                foreach (var permission in permissions)
                {
                    role.AddPermission(permission);
                }

            }

            var result = await _roleRepository.SaveAsync();
            if(result)
            return new(true);

            return new(false, ValidationMessages.SystemErrorMessage, "Role");

        }

        public async Task<OperationResult> EditUserRoleAsync(int userId, List<int> roles)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            user.ClearRole();
            foreach (var role in roles)
            {
                user.AddRole(role);
            }
            var result = await _roleRepository.SaveAsync();
            if (result)
                return new(true);

            return new(false, ValidationMessages.SystemErrorMessage, "User");
        }
    }
}
