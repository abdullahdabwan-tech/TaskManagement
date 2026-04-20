using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.DTOs.Users;

namespace TaskManagement.Application.Interfaces
{
    public interface IUsersService
    {
        Task<int> CreateAsync(CreateUserDto dto);
        Task<List<UserListItemDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateUserDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
