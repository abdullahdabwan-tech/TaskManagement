namespace TaskManagement.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;

        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

}
