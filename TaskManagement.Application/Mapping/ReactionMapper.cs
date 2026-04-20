using TaskManagement.Application.DTOs.Reactions;
using TaskManagement.Domain;

namespace TaskManagement.Application.Mapping
{
    public static class ReactionMapper
    {
        public static ReactionDto ToDto(Reaction reaction)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            return new ReactionDto
            {
                Id = reaction.Id,
                UserName = reaction.User?.UserName,
                ReactionType = reaction.ReactionType?.Name,
                TaskId = reaction.TaskId,
                CommentId = reaction.CommentId,
                CreatedAt = reaction.CreatedAt
            };
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        public static Reaction ToEntity(CreateReactionDto dto)
        {
            return new Reaction
            {
                UserId = dto.UserId,
                ReactionTypeId = dto.ReactionTypeId,
                TaskId = dto.TaskId,
                CommentId = dto.CommentId,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
