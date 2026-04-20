namespace TaskManagement.Application.DTOs.Tasks
{
    public class TaskDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }

        public string? Status { get; set; }
        public string? Priority { get; set; }

        public required string UserName { get; set; }
        public string? CategoryName { get; set; }

        public int CommentsCount { get; set; }
        public int ReactionsCount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
