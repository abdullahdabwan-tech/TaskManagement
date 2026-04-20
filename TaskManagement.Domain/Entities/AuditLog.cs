using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        public required string Action { get; set; }
        public required string Entity { get; set; }
        public string? IpAddress { get; set; }

        public int? EntityId { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
