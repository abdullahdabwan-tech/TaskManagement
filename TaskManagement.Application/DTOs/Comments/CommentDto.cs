namespace TaskManagement.Application.DTOs.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public required string Content { get; set; }

        public required string UserName { get; set; }
        public int TaskId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
