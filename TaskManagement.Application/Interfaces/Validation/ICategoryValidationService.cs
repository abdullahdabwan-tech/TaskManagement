namespace TaskManagement.Application.Interfaces.Validation
{
    public interface ICategoryValidationService
    {
        Task<bool> ExistsAsync(int? categoryId);
    }
}
