using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Application.DTOs.Comments
{
    public class CreateCommentDto
    {
        public required string Content { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
    }
}
