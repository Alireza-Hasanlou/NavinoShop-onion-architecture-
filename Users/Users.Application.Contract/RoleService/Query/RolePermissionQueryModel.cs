using Shared.Domain.Enums;
using System.Security;

namespace Users.Application.Contract.RoleService.Query
{
    public class RolePermissionQueryModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public List<PermissionDto> Permissions{ get; set; }
    }
}