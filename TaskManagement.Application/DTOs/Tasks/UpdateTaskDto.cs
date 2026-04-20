using TaskManagement.Domain;

namespace TaskManagement.Application.DTOs.Tasks
{
    public class UpdateTaskDto
    {
        public required string Title { get; set; }
        public Domain.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public int? CategoryId { get; set; }
    }
}
