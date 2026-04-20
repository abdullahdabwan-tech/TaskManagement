namespace TaskManagement.Application.Security.Permissions
{
    public interface IPermissionResolver
    {
        Task<HashSet<string>> GetUserPermissionsAsync(int userId);
    }
}
