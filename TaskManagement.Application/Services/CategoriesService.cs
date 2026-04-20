using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.DTOs.Categories;
using TaskManagement.Application.DTOs.Comments;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Mapping;
using TaskManagement.Domain;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure;

namespace TaskManagement.Application.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly AppDbContext _context;
        private readonly IAuditLogService _auditLogService;

        public CategoriesService(AppDbContext context, IAuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService;
        }
        public async Task<int> CreateAsync(CreateCategoryDto dto, int userId)
        {
            var exists = await _context.Categories.AnyAsync(x => x.Name == dto.Name);

            if (exists)
                throw new BusinessException("Category already exists");

            var category = new Category
            {
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow
            };

            _context.Categories.Add(category);


            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync(
                "CREATE",
                "Category",
                category.Id,
                null
            );

            return category.Id;
        }

        public async Task<List<CategoryListItemDto>> GetAllAsync()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync();

            return categories.Select(CategoryMapper.ToListDto).ToList();
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return category == null ? null : CategoryMapper.ToDto(category);
        }
        public async Task<bool> UpdateAsync(int id, UpdateCategoryDto dto, int userId)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return false;

            category.Name = dto.Name;



            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(
               "UPDATE",
               "Category",
               category.Id,
               null
           );
            return true;
        }
        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return false;

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync(
                "DELETE",
                "Category",
                category.Id,
                null
            );
            return true;
        }
    }
}
