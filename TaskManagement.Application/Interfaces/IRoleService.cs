using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Application.DTOs.Roles;

namespace TaskManagement.Application.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllAsync();
        Task<RoleDto?> GetByIdAsync(int id);

        Task<int> CreateAsync(CreateRoleDto dto);
        Task<bool> UpdateAsync(int id, UpdateRoleDto dto);
        Task<bool> DeleteAsync(int id);

        Task AssignPermissionAsync(AssignPermissionDto dto);
        Task RemovePermissionAsync(AssignPermissionDto dto);
    }
}
