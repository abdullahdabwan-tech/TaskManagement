using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain;
using TaskManagement.Infrastructure;
using static TaskManagement.Application.Security.Permissions.PermissionAttribute;

namespace TaskManagement.Application.Security.Permissions
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionResolver _resolver;

        public PermissionHandler(IPermissionResolver resolver)
        {
            _resolver = resolver;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var userIdClaim = context.User.FindFirst("UserId");

            if (userIdClaim == null)
                return;

            var userId = int.Parse(userIdClaim.Value);

            var permissions = await _resolver.GetUserPermissionsAsync(userId);

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
