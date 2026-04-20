using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Domain;

namespace TaskManagement.Application.DTOs.Tasks
{
    public class CreateTaskDto
    {
        public required string Title { get; set; }
        public int UserId { get; set; }
        public int? CategoryId { get; set; }
        public TaskPriority Priority { get; set; }
        public Domain.TaskStatus Status { get; set; }
    }
}
