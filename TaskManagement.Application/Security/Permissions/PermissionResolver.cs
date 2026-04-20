using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure;

namespace TaskManagement.Application.Security.Permissions
{
    public class PermissionResolver : IPermissionResolver
    {
        private readonly AppDbContext _context;
        private readonly IPermissionCacheService _cache;

        public PermissionResolver(AppDbContext context, IPermissionCacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<HashSet<string>> GetUserPermissionsAsync(int userId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return new HashSet<string>();

            return await _cache.GetPermissionsAsync(user.RoleId);
        }
    }
}
