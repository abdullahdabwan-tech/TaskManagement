using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Application.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public List<string>? Errors { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "")
            => new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };

        public static ApiResponse<T> Fail(string message, List<string>? errors = null)
            => new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
    }
}
