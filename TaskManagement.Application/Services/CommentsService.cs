using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.DTOs.Comments;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Mapping;
using TaskManagement.Domain;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure;

namespace TaskManagement.Application.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly AppDbContext _context;
        private readonly IAuditLogService _auditLogService;

        public CommentsService(AppDbContext context, IAuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService;
        }
        public async Task<int> CreateAsync(CreateCommentDto dto, int userId)
        {
            var taskExists = await _context.Tasks
                .AnyAsync(x => x.Id == dto.TaskId);

            if (!taskExists)
                throw new BusinessException("Task not found");

            var comment = new Comment
            {
                Content = dto.Content,
                TaskId = dto.TaskId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync(
                "CREATE",
                "Comment",
                comment.Id,
                userId
            );

            return comment.Id;
        }
        public async Task<List<CommentDto>> GetByTaskIdAsync(int taskId)
        {
            var comments = await _context.Comments
                .AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.TaskId == taskId)
                .ToListAsync();

            return comments.Select(CommentMapper.ToDto).ToList();
        }

        public async Task<CommentDto?> GetByIdAsync(int id)
        {
            var comment = await _context.Comments
                .AsNoTracking()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            return comment == null ? null : CommentMapper.ToDto(comment);
        }

        public async Task<bool> UpdateAsync(int id, UpdateCommentDto dto, int userId)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
                return false;

            if (comment.UserId != userId)
                throw new BusinessException("Not allowed");

            comment.Content = dto.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync(
                "UPDATE",
                "Comment",
                comment.Id,
                userId
            );

            return true;
        }
        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
                return false;

            if (comment.UserId != userId)
                throw new BusinessException("Not allowed");

            _context.Comments.Remove(comment);

            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync(
                "DELETE",
                "Comment",
                comment.Id,
                userId
            );

            return true;
        }
    }
}
