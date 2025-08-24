using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contract.PermissionService.Command;
using Users.Domain.User.Agg;
using Users.Domain.User.Agg.IRepository;
using Utility.Shared.Application;

namespace Users.Application.Services
{
    internal class PermissionService : IPermissionCommandService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<OperationResult> Create(CreatePermissionCommand command)
        {
            if (string.IsNullOrEmpty(command.Title))
                return new(false, ValidationMessages.RequiredMessage, "Title");
            if (await _permissionRepository.ExistByAsync(t => t.Title == command.Title.Trim()))
                return new(false, ValidationMessages.DuplicatedMessage, "Title");

            var res = await _permissionRepository.CreateAsync(new Permission(command.Title, command.Description));
            if (res.Success)
                return new(true);

            return new(false, ValidationMessages.SystemErrorMessage, "Permission");
        }

        public async Task<OperationResult> Edit(EditPermissionCommand command)
        {

            if (string.IsNullOrEmpty(command.Title))
                return new(false, ValidationMessages.RequiredMessage, "Title");
            if (await _permissionRepository.ExistByAsync(t => t.Title == command.Title.Trim() && t.Id != command.Id))
                return new(false, ValidationMessages.DuplicatedMessage, "Title");

            var permission = await _permissionRepository.GetByIdAsync(command.Id);

            string Description = permission.Description;
            if (!string.IsNullOrEmpty(command.Description))
                Description = command.Description.Trim();

            permission.Edit(command.Title.Trim(), Description);
            var res = await _permissionRepository.SaveAsync();
            if(res)
                return new(true);

            return new(false, ValidationMessages.SystemErrorMessage, "Permission");
        }
    }
}
