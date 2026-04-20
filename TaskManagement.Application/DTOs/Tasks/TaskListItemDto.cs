namespace TaskManagement.Application.DTOs.Tasks
{
    public class TaskListItemDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }

        public required string Status { get; set; }
        public required string Priority { get; set; }

        public required string UserName { get; set; }
    }
}
