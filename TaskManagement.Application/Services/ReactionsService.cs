using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.DTOs.Reactions;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Mapping;
using TaskManagement.Domain;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure;

namespace TaskManagement.Application.Services
{
    public class ReactionsService : IReactionsService
    {
        private readonly AppDbContext _context;
        private readonly IAuditLogService _auditLogService;

        public ReactionsService(AppDbContext context, IAuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService;
        }
        public async Task<int> CreateAsync(CreateReactionDto dto, int userId)
        {
            if (!dto.TaskId.HasValue && !dto.CommentId.HasValue)
                throw new BusinessException("Reaction must belong to Task or Comment");

            if (dto.TaskId.HasValue && dto.CommentId.HasValue)
                throw new BusinessException("Invalid reaction target");

            if (dto.TaskId.HasValue)
            {
                var taskExists = await _context.Tasks
                    .AnyAsync(x => x.Id == dto.TaskId);

                if (!taskExists)
                    throw new BusinessException("Task not found");
            }

            if (dto.CommentId.HasValue)
            {
                var commentExists = await _context.Comments
                    .AnyAsync(x => x.Id == dto.CommentId);

                if (!commentExists)
                    throw new BusinessException("Comment not found");
            }

            var duplicate = await _context.Reactions.AnyAsync(x =>
                x.UserId == userId &&
                x.TaskId == dto.TaskId &&
                x.CommentId == dto.CommentId &&
                x.ReactionTypeId == dto.ReactionTypeId);

            if (duplicate)
                throw new BusinessException("Already reacted");

            var reaction = new Reaction
            {
                UserId = userId,
                TaskId = dto.TaskId,
                CommentId = dto.CommentId,
                ReactionTypeId = dto.ReactionTypeId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reactions.Add(reaction);
            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync(
                "CREATE",
                "Reaction",
                reaction.Id,
                userId
            );

            return reaction.Id;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var reaction = await _context.Reactions
                .FirstOrDefaultAsync(x => x.Id == id);

            if (reaction == null)
                return false;

            if (reaction.UserId != userId)
                throw new BusinessException("Not allowed");

            _context.Reactions.Remove(reaction);
            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync(
                "DELETE",
                "Reaction",
                reaction.Id,
                userId
            );

            return true;
        }

        public async Task<List<ReactionDto>> GetByTaskIdAsync(int taskId)
        {
            var reactions = await _context.Reactions
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.ReactionType)
                .Where(x => x.TaskId == taskId)
                .ToListAsync();

            return reactions.Select(ReactionMapper.ToDto).ToList();
        }
        public async Task<List<ReactionDto>> GetByCommentIdAsync(int commentId)
        {
            var reactions = await _context.Reactions
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.ReactionType)
                .Where(x => x.CommentId == commentId)
                .ToListAsync();

            return reactions.Select(ReactionMapper.ToDto).ToList();
        }




    }
}
