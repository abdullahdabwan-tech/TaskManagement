namespace TaskManagement.Domain
{
    public class Reaction
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ReactionTypeId { get; set; }
        public ReactionType ReactionType { get; set; } = null!;

        public int? TaskId { get; set; }
        public TaskItem? Task { get; set; }

        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}