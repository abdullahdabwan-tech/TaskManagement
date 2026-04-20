using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Common.Query;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Mapping;
using TaskManagement.Domain;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly IAuditLogService _auditLogService;

        public TaskService(AppDbContext context, IAuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService;
        }

        public async Task<int> CreateAsync(CreateTaskDto dto, int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new BusinessException("User not found");

            if (dto.CategoryId.HasValue)
            {
                var categoryExists = await _context.Categories
                    .AnyAsync(x => x.Id == dto.CategoryId);

                if (!categoryExists)
                    throw new BusinessException("Category not found");
            }

            var duplicate = await _context.Tasks.AnyAsync(x =>
                x.Title == dto.Title && x.UserId == userId);

            if (duplicate)
                throw new BusinessException("Task already exists");

            var task = new TaskItem
            {
                Title = dto.Title,
                Status = Domain.TaskStatus.Pending,
                Priority = TaskPriority.Medium,
                UserId = userId,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync("CREATE", "Task", task.Id, userId);

            return task.Id;
        }
        public async Task<List<TaskDto>> GetAllAsync(int userId)
        {
            var tasks = await _context.Tasks
                          .AsNoTracking()
                          .Where(x => x.UserId == userId && !x.IsDeleted)
                          .Include(x => x.User)
                          .ToListAsync();

            return tasks.Select(TaskMapper.ToDto).ToList();
        }

        public async Task<List<TaskListItemDto>> GetAllViewAsync(int userId)
        {
            var tasks = await _context.Tasks
                           .AsNoTracking()
                           .Where(x => x.UserId == userId && !x.IsDeleted)
                           .Include(x => x.User)
                           .ToListAsync();

            return tasks.Select(TaskMapper.ToListDto).ToList();
        }

        public async Task<TaskDto?> GetByIdAsync(int id)
        {
            var task = await _context.Tasks
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Category)
                .Include(x => x.Comments)
                .Include(x => x.Reactions)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            return task == null ? null : TaskMapper.ToDto(task);
        }

        public async Task<bool> UpdateAsync(int id, UpdateTaskDto dto, int userId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null || task.IsDeleted)
                throw new BusinessException("Task not found");

            if (task.UserId != userId)
                throw new BusinessException("Not allowed");

            task.Title = dto.Title;
            task.Status = dto.Status;
            task.Priority = dto.Priority;
            task.CategoryId = dto.CategoryId;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync("UPDATE", "Task", task.Id, userId);

            return true;
        }


        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
                throw new BusinessException("Task not found");

            if (task.UserId != userId)
                throw new BusinessException("Not allowed");

            task.IsDeleted = true;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync("DELETE", "Task", id, userId);

            return true;
        }

        public async Task<PagedResult<TaskListItemDto>> GetPagedAsync(TaskQueryDto query)
        {
            var data = _context.Tasks
                .AsNoTracking()
                .AsQueryable();
            if (!string.IsNullOrEmpty(query.Search))
            {
                data = data.Where(x =>
                    x.Title.Contains(query.Search));
            }
            if (!string.IsNullOrEmpty(query.Status))
                data = data.Where(x => x.Status.ToString() == query.Status);

            if (!string.IsNullOrEmpty(query.Priority))
                data = data.Where(x => x.Priority.ToString() == query.Priority);

            if (query.CategoryId.HasValue)
                data = data.Where(x => x.CategoryId == query.CategoryId);

            if (query.UserId.HasValue)
                data = data.Where(x => x.UserId == query.UserId);

            data = query.SortBy switch
            {
                "Title" => query.IsDescending
                    ? data.OrderByDescending(x => x.Title)
                    : data.OrderBy(x => x.Title),

                "Priority" => query.IsDescending
                    ? data.OrderByDescending(x => x.Priority)
                    : data.OrderBy(x => x.Priority),

                _ => query.IsDescending
                    ? data.OrderByDescending(x => x.CreatedAt)
                    : data.OrderBy(x => x.CreatedAt)
            };
            var total = await data.CountAsync();

            var items = await data
                            .Skip((query.Page - 1) * query.PageSize)
                            .Take(query.PageSize)
                            .ToListAsync();

            return new PagedResult<TaskListItemDto>
            {
                Items = items.Select(TaskMapper.ToListDto).ToList(),
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }


    }
}
