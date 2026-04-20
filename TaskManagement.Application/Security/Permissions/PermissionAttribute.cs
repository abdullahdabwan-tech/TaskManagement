using Microsoft.AspNetCore.Authorization;

namespace TaskManagement.Application.Security.Permissions
{
    public partial class PermissionAttribute : AuthorizeAttribute
    {
        public PermissionAttribute(string permission)
        {
            Policy = null;
            AuthenticationSchemes = "Bearer";
        }
    }
}
