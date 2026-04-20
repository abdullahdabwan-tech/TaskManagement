using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }
        public string? RevokedReason { get; set; }

        public string? CreatedByIp { get; set; }
        public string? Device { get; set; }
        public bool IsRevoked => RevokedAt != null || DateTime.UtcNow >= ExpiresAt;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
