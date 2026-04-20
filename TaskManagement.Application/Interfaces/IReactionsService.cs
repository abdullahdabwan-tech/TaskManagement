using TaskManagement.Application.DTOs.Reactions;

namespace TaskManagement.Application.Interfaces
{
    public interface IReactionsService
    {
        Task<int> CreateAsync(CreateReactionDto dto, int userId);

        Task<bool> DeleteAsync(int id, int userId);

        Task<List<ReactionDto>> GetByTaskIdAsync(int taskId);

        Task<List<ReactionDto>> GetByCommentIdAsync(int commentId);
    }
}
