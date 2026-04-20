using TaskManagement.Application.Common.Query;
using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskService
    {
        Task<int> CreateAsync(CreateTaskDto dto, int userId);
        Task<TaskDto?> GetByIdAsync(int id);
        Task<List<TaskDto>> GetAllAsync(int userId);
        Task<List<TaskListItemDto>> GetAllViewAsync(int userId);
        Task<bool> UpdateAsync(int id, UpdateTaskDto dto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
        Task<PagedResult<TaskListItemDto>> GetPagedAsync(TaskQueryDto query);

    }
}
