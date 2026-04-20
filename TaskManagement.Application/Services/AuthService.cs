using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Common.Security;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.DTOs.RefreshToken;
using TaskManagement.Application.Security;
using TaskManagement.Domain;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure;

namespace TaskManagement.Application.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwt;
        private readonly TokenService _tokenService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly PasswordService _passwordService;
        private readonly LoginAttemptService _loginAttempt;

        public AuthService(
            AppDbContext context,
            JwtService jwt,
            TokenService tokenService,
            IHttpContextAccessor httpContext,
            PasswordService passwordService,
            LoginAttemptService loginAttempt
            )
        {
            _context = context;
            _jwt = jwt;
            _tokenService = tokenService;
            _httpContext = httpContext;
            _passwordService = passwordService;
            _loginAttempt = loginAttempt;
        }

        // 🔥 تحميل المستخدم مع كل العلاقات
        private IQueryable<User> GetUserWithPermissions()
        {
            return _context.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission);
        }

        // =========================================
        // REGISTER
        // =========================================
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var exists = await _context.Users
                .AnyAsync(x => x.Email == dto.Email);

            if (exists)
                throw new BusinessException("Email already exists");

            var defaultRole = await _context.Roles
                .FirstOrDefaultAsync(x => x.Name == "User");

            if (defaultRole == null)
                throw new BusinessException("Default role not found");

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                RoleId = defaultRole.Id,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordService.Hash(user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 🔥 إعادة تحميل المستخدم مع العلاقات
            var userWithRelations = await GetUserWithPermissions()
                .FirstAsync(u => u.Id == user.Id);

            return await GenerateAuthResponse(userWithRelations, "REGISTER");
        }

        // =========================================
        // LOGIN
        // =========================================
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await GetUserWithPermissions()
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                throw new BusinessException("Invalid credentials");
            var ip = _httpContext.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            if (_loginAttempt.IsBlocked(ip))
                throw new BusinessException("Too many attempts. Try again later.");

            if (!_passwordService.Verify(user, dto.Password))
            {
                //_loginAttempt.Fail(ip);
                throw new BusinessException("Invalid credentials");
            }

            _loginAttempt.Reset(ip);
            return await GenerateAuthResponse(user, "LOGIN");
        }

        // =========================================
        // REFRESH TOKEN
        // =========================================
        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            var hashed = _tokenService.HashToken(dto.RefreshToken);

            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == hashed);

            if (storedToken == null)
                throw new BusinessException("Invalid refresh token");

            if (storedToken.IsRevoked)
                throw new BusinessException("Refresh token expired or revoked");

            if (storedToken.RevokedAt != null)
                throw new BusinessException("Token already used");
            // revoke القديم
            storedToken.RevokedAt = DateTime.UtcNow;
            storedToken.RevokedReason = "Replaced";

            // 🔥 تحميل المستخدم مع العلاقات
            var user = await GetUserWithPermissions()
                .FirstAsync(u => u.Id == storedToken.UserId);

            return await GenerateAuthResponse(user, "REFRESH_TOKEN");
        }

        // =========================================
        // CORE (DRY)
        // =========================================
        private async Task<AuthResponseDto> GenerateAuthResponse(User user, string action)
        {
            var permissions = await _context.RolePermissions
    .Where(x => x.RoleId == user.RoleId)
    .Select(x => x.Permission.Name)
    .ToListAsync();


            var accessToken = _jwt.GenerateToken(user, permissions);

            var rawToken = _tokenService.GenerateRefreshToken();
            var hashedToken = _tokenService.HashToken(rawToken);

            var ip = _httpContext.HttpContext?.Connection.RemoteIpAddress?.ToString();
            var device = _httpContext.HttpContext?.Request.Headers["User-Agent"].ToString();

            var refreshEntity = new RefreshToken
            {
                Token = hashedToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ip,
                Device = device
            };

            _context.RefreshTokens.Add(refreshEntity);

            await _context.AuditLogs.AddAsync(new AuditLog
            {
                UserId = user.Id,
                Action = action,
                Entity = "Auth",
                EntityId = user.Id,
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = rawToken,
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }

}