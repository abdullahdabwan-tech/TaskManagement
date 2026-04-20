using TaskManagement.Application.DTOs.Users;
using TaskManagement.Domain;

namespace TaskManagement.Application.Mapping
{
    public static class UserMapper
    {
        public static UserDto ToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsActive = user.IsActive
            };
        }

        public static UserListItemDto ToListDto(User user)
        {
            return new UserListItemDto
            {
                Id = user.Id,
                UserName = user.UserName
            };
        }

        public static User ToEntity(CreateUserDto dto)
        {
            return new User
            {
                UserName = dto.UserName,
                Email = dto.Email
            };
        }
    }
}