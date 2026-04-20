using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Application.Common.Query;

namespace TaskManagement.Application.DTOs.Tasks
{
    public class TaskQueryDto : PaginationParams
    {
        public string? Search { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public int? CategoryId { get; set; }
        public int? UserId { get; set; }

        public string? SortBy { get; set; } = "CreatedAt";
        public bool IsDescending { get; set; } = true;
    }
}
