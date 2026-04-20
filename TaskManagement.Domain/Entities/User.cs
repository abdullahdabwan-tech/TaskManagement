using System.Security.Claims;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain
{
    public class User
    {
        public int Id { get; set; }

        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? PasswordHash { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
