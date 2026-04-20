namespace TaskManagement.Application.DTOs.Reactions
{
    public class ReactionDto
    {
        public int Id { get; set; }

        public required string UserName { get; set; }
        public required string ReactionType { get; set; }

        public int? TaskId { get; set; }
        public int? CommentId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
