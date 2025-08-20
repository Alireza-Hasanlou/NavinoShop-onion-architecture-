using System;
using System.Collections.Generic;
using System.Linq;
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

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<OperationResult> CreateAsync(CreateRoleCommand command, List<int> permissions)
        {
            if (await _roleRepository.ExistByAsync(t => t.Title == command.Title))
                return new(false);

            var result = await _roleRepository.CreateAsync(new Role(command.Title));
            if (result.Success)
            {
                if (_roleRepository.AddPermissionsToRole(command.Title, permissions))
                    return new(true);
            }

            return new(false);
        }

        public async Task<OperationResult> EditAsync(EditRoleCommand command, List<int> permissions)
        {
            var role = await _roleRepository.GetByIdAsync(command.Id);
            if (await _roleRepository.ExistByAsync(t => t.Title == command.Title && role.Id != command.Id))
                return new(false,ValidationMessages.DuplicatedMessage,"Role");


            if (_roleRepository.AddPermissionsToRole(command.Title, permissions))
            {
                role.Edit(command.Title);
                _roleRepository.SaveAsync();
            }

            return new(false,ValidationMessages.SystemErrorMessage);

        }

        public Task<OperationResult> EditUserRoleAsync(int userId, List<int> roles)
        {
            throw new NotImplementedException();
        }
    }
}
