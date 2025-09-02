using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Domain.Enums;
using System.Security.Claims;
using Users.Application.Contract.RoleService.Query;


namespace NavinoShop.WebApplication.Utility
{

    internal class PermissionChecker : AuthorizeAttribute, IAuthorizationFilter
    {
        private IRoleQueryService _roleQuery;
        private readonly int _permissionId;
        public PermissionChecker(int permissionId)
        {
            _permissionId = permissionId;

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _roleQuery = (IRoleQueryService)context.HttpContext.RequestServices.
               GetService(typeof(IRoleQueryService));
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = context.HttpContext.User.Claims.
                FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                if (_roleQuery.CheckPermission(int.Parse(userId), _permissionId) == false)
                    context.Result = new ViewResult
                    {
                        ViewName = "/Views/Shared/AccessDenied.cshtml"
                    };
                context.HttpContext.Response.StatusCode = 403;

            }
            else
                context.Result = new RedirectResult("/Account/Login");
        }
    }
}
