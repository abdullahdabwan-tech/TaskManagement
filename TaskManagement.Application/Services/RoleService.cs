using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.DTOs.Roles;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure;

namespace TaskManagement.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;
        private readonly IAuditLogService _auditLogService;

        public RoleService(AppDbContext context, IAuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService;
        }

        public async Task<List<RoleDto>> GetAllAsync()
        {
            return await _context.Roles
                .AsNoTracking()
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Permissions = r.RolePermissions
                        .Select(p => p.Permission.Name)
                        .ToList()
                })
                .ToListAsync();
        }
        public async Task<RoleDto?> GetByIdAsync(int id)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
                return null;

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = role.RolePermissions
                    .Select(p => p.Permission.Name)
                    .ToList()
            };
        }

        public async Task<int> CreateAsync(CreateRoleDto dto)
        {
            var exists = await _context.Roles.AnyAsync(x => x.Name == dto.Name);

            if (exists)
                throw new BusinessException("Role already exists");

            var role = new Role
            {
                Name = dto.Name
            };

            _context.Roles.Add(role);

            _context.AuditLogs.Add(new AuditLog
            {
                Action = "Create",
                Entity = "Role",
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return role.Id;
        }

        public async Task<bool> UpdateAsync(int id, UpdateRoleDto dto)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                return false;

            role.Name = dto.Name;

            _context.AuditLogs.Add(new AuditLog
            {
                Action = "Update",
                Entity = "Role",
                EntityId = role.Id,
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (role == null)
                return false;

            _context.RolePermissions.RemoveRange(role.RolePermissions);
            _context.Roles.Remove(role);

            _context.AuditLogs.Add(new AuditLog
            {
                Action = "Delete",
                Entity = "Role",
                EntityId = role.Id,
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task AssignPermissionAsync(AssignPermissionDto dto)
        {
            var exists = await _context.RolePermissions
                .AnyAsync(x => x.RoleId == dto.RoleId &&
                               x.PermissionId == dto.PermissionId);

            if (exists)
                return;

            var rp = new RolePermission
            {
                RoleId = dto.RoleId,
                PermissionId = dto.PermissionId
            };

            _context.RolePermissions.Add(rp);
            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync(
                "ASSIGN_PERMISSION",
                "RolePermission",
                null,
                null
            );
        }
        public async Task RemovePermissionAsync(AssignPermissionDto dto)
        {
            var rp = await _context.RolePermissions
                .FirstOrDefaultAsync(x =>
                    x.RoleId == dto.RoleId &&
                    x.PermissionId == dto.PermissionId);

            if (rp == null)
                return;

            _context.RolePermissions.Remove(rp);
            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync(
                "REMOVE_PERMISSION",
                "RolePermission",
                null,
                null
            );
        }
    }
}
