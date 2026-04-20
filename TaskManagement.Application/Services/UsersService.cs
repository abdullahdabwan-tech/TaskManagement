using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Mapping;
using TaskManagement.Application.Security;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure;

namespace TaskManagement.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext _context;
        private readonly IAuditLogService _auditLogService;
        private readonly PasswordService _passwordService;

        public UsersService(AppDbContext context, IAuditLogService auditLogService, PasswordService password)
        {
            _context = context;
            _auditLogService = auditLogService;
            _passwordService = password;
        }
        public async Task<int> CreateAsync(CreateUserDto dto)
        {
            var exists = await _context.Users
                .AnyAsync(x => x.Email == dto.Email);

            if (exists)
                throw new BusinessException("Email already exists");

            var user = UserMapper.ToEntity(dto);
            user.PasswordHash = _passwordService.Hash(user, dto.Password);
            user.CreatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // مهم قبل Audit

            await _auditLogService.LogAsync(
                "CREATE",
                "User",
                user.Id,
                user.Id
            );

            return user.Id;
        }

        public async Task<List<UserListItemDto>> GetAllAsync()
        {
            var users = await _context.Users
                .AsNoTracking()
                .Where(x => x.IsActive)
                .ToListAsync();

            return users.Select(UserMapper.ToListDto).ToList();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                throw new BusinessException("User not found");

            if (!user.IsActive)
                throw new BusinessException("User is inactive");

            return UserMapper.ToDto(user);
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new BusinessException("User not found");

            user.UserName = dto.UserName;
            user.Email = dto.Email;
            user.IsActive = dto.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync(
                "UPDATE",
                "User",
                user.Id,
                user.Id
            );

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new BusinessException("User not found");

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync(
                "DELETE",
                "User",
                user.Id,
                user.Id
            );

            return true;
        }
    }
}
