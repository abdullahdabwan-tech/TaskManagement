using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
