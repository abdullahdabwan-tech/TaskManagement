namespace TaskManagement.Application.Interfaces.Validation
{
    public interface ITaskValidationService
    {
        Task<bool> ExistsAsync(int taskId);
    }
}
