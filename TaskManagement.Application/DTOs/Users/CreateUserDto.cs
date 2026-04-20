using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Application.DTOs.Users
{
    public class CreateUserDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
