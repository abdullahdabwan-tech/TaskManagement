using FluentValidation;
using TaskManagement.Application.Common;
using TaskManagement.Application.Common.Exceptions;

namespace TaskManagement.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.Clear();
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<string>.Fail(
                        "Validation error",
                        errors
                    )
                );
            }
            catch (BusinessException ex)
            {
                context.Response.Clear();
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<string>.Fail(ex.Message)
                );
            }
            catch (UnauthorizedAccessException)
            {
                context.Response.Clear();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<string>.Fail("Unauthorized access")
                );
            }
            catch (NotFoundException ex)
            {
                context.Response.Clear();
                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<string>.Fail(ex.Message)
                );
            }
            catch (Exception ex)
            {
                context.Response.Clear();
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                _logger.LogError(ex, "Unhandled exception occurred");

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<string>.Fail(
                        "Internal server error",
                        new List<string> { ex.Message }
                    )
                );
            }
        }
    }
}