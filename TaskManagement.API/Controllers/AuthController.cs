using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.DTOs.RefreshToken;
using TaskManagement.Application.Services;

namespace TaskManagement.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("global")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _service.RegisterAsync(dto);

            return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, "Registered successfully"));
        }


        [HttpPost("login")]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _service.LoginAsync(dto);

            return Ok(ApiResponse<AuthResponseDto>
           .SuccessResponse(result, "Login successful"));
        }



        [HttpPost("refresh")]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> Refresh(RefreshTokenDto dto)
        {
            var result = await _service.RefreshTokenAsync(dto);

            return Ok(ApiResponse<AuthResponseDto>
                .SuccessResponse(result, "Token refreshed"));
        }
    }
}
