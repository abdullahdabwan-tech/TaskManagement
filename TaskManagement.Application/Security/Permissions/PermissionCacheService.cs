using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TaskManagement.Infrastructure;

namespace TaskManagement.Application.Security.Permissions
{
    public class PermissionCacheService : IPermissionCacheService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        private const string KeyPrefix = "role_permissions_";

        public PermissionCacheService(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<HashSet<string>> GetPermissionsAsync(int roleId)
        {
            var cacheKey = $"{KeyPrefix}{roleId}";

            if (_cache.TryGetValue(cacheKey, out HashSet<string> permissions))
                return permissions;

            permissions = await _context.RolePermissions
                .Where(x => x.RoleId == roleId)
                .Select(x => x.Permission.Name)
                .ToHashSetAsync();

            _cache.Set(cacheKey, permissions, TimeSpan.FromMinutes(30));

            return permissions;
        }

        public Task RefreshRoleAsync(int roleId)
        {
            _cache.Remove($"{KeyPrefix}{roleId}");
            return Task.CompletedTask;
        }
    }
}
