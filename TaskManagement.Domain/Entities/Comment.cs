namespace TaskManagement.Domain
{
    public class Comment
    {
        public int Id { get; set; }

        public required string Content { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int TaskId { get; set; }
        public TaskItem Task { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}
