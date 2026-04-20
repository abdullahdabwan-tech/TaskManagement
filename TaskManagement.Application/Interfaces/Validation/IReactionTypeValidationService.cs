namespace TaskManagement.Application.Interfaces.Validation
{
    public interface IReactionTypeValidationService
    {
        Task<bool> ExistsAsync(int reactionTypeId);
    }
}
