using TaskManagement.Application.DTOs.Comments;

namespace TaskManagement.Application.Interfaces
{
    public interface ICommentsService
    {
        Task<int> CreateAsync(CreateCommentDto dto, int userId);

        Task<List<CommentDto>> GetByTaskIdAsync(int taskId);

        Task<CommentDto?> GetByIdAsync(int id);

        Task<bool> UpdateAsync(int id, UpdateCommentDto dto, int userId);

        Task<bool> DeleteAsync(int id, int userId);
    }
}
