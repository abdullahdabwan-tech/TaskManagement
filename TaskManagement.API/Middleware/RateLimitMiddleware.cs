using TaskManagement.Application.Common;
using TaskManagement.Application.Security.RateLimiting;

namespace TaskManagement.API.Middleware
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;

        public RateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
       HttpContext context,
       UserRateLimiter limiter)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userId = context.User.FindFirst("UserId")?.Value;

            // 🔥 تحديد الحد حسب endpoint
            int limit = 20;

            if (context.Request.Path.ToString().Contains("/login"))
            {
                limit = 5;
            }

            string key = userId != null
                ? $"user:{userId}"
                : $"ip:{ip}";

            if (limiter.IsLimited(key))
            {
                context.Response.StatusCode = 429;

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<string>.Fail("Too many requests, slow down.")
                );

                return;
            }

            await _next(context);
        }
    }
}
