namespace TaskManagement.Application.DTOs.Users
{
    public class UpdateUserDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required bool IsActive { get; set; }
    }
}
