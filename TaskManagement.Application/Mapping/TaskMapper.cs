using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Domain;

namespace TaskManagement.Application.Mapping
{
    public static class TaskMapper
    {
        public static TaskDto ToDto(TaskItem task)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Status = task.Status.ToString(),
                Priority = task.Priority.ToString(),
                UserName = task.User?.UserName,
                CommentsCount = task.Comments?.Count ?? 0,
                ReactionsCount = task.Reactions?.Count ?? 0,
                CreatedAt = task.CreatedAt,
            };
#pragma warning restore CS8601 // Possible null reference assignment.
        }
        public static TaskListItemDto ToListDto(TaskItem task)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            return new TaskListItemDto
            {
                Id = task.Id,
                Title = task.Title,
                Status = task.Status.ToString(),
                Priority = task.Priority.ToString(),

                UserName = task.User?.UserName
            };
#pragma warning restore CS8601 // Possible null reference assignment.
        }
        public static TaskItem ToEntity(CreateTaskDto dto)
        {
            return new TaskItem
            {
                Title = dto.Title,
                UserId = dto.UserId,
                CategoryId = dto.CategoryId,
                Priority = dto.Priority,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
