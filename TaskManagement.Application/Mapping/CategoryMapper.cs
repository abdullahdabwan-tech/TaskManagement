using TaskManagement.Application.DTOs.Categories;
using TaskManagement.Domain;

namespace TaskManagement.Application.Mapping
{
    public static class CategoryMapper
    {
        public static CategoryDto ToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public static CategoryListItemDto ToListDto(Category category)
        {
            return new CategoryListItemDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public static Category ToEntity(CreateCategoryDto dto)
        {
            return new Category
            {
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
