using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Application.Interfaces.Validation
{
    public interface IUserValidationService
    {
        Task<bool> ExistsAsync(int userId);
        Task<bool> EmailExistsAsync(string email);
    }
}
