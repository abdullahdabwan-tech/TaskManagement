using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Application.Security.Permissions
{
    public interface IPermissionCacheService
    {
        Task<HashSet<string>> GetPermissionsAsync(int roleId);
        Task RefreshRoleAsync(int roleId);
    }
}
