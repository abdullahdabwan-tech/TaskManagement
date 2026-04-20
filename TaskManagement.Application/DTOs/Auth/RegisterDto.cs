using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Application.DTOs.Auth
{
    public class RegisterDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

}
