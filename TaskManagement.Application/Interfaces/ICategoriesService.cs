using TaskManagement.Application.DTOs.Categories;
using TaskManagement.Application.DTOs.Tasks;

namespace TaskManagement.Application.Interfaces
{
    public interface ICategoriesService
    {
        Task<int> CreateAsync(CreateCategoryDto dto, int userId);

        Task<List<CategoryListItemDto>> GetAllAsync();

        Task<CategoryDto?> GetByIdAsync(int id);

        Task<bool> UpdateAsync(int id, UpdateCategoryDto dto, int userId);

        Task<bool> DeleteAsync(int id, int userId);
    }
}
