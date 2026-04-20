using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Application.DTOs.Reactions
{
    public class CreateReactionDto
    {
        public int UserId { get; set; }
        public int ReactionTypeId { get; set; }

        public int? TaskId { get; set; }
        public int? CommentId { get; set; }
    }
}
