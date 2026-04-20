using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.DTOs.RefreshToken;

namespace TaskManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto);
    }
}
