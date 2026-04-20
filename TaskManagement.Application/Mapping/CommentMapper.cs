using TaskManagement.Application.DTOs.Comments;
using TaskManagement.Domain;

namespace TaskManagement.Application.Mapping
{
    public static class CommentMapper
    {
        public static CommentDto ToDto(Comment comment)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            return new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                UserName = comment.User?.UserName,
                TaskId = comment.TaskId,
                CreatedAt = comment.CreatedAt
            };
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        public static Comment ToEntity(CreateCommentDto dto)
        {
            return new Comment
            {
                Content = dto.Content,
                TaskId = dto.TaskId,
                UserId = dto.UserId,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
