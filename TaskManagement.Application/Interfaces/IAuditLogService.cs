namespace TaskManagement.Application.Interfaces
{
    public interface IAuditLogService
    {
        Task LogAsync(string action, string entity, int? entityId, int? userId);
    }

}
