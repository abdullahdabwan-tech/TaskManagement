using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Categories;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Security.Permissions;

namespace TaskManagement.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _service;

        public CategoriesController(ICategoriesService service)
        {
            _service = service;
        }

        private int GetUserId()
            => int.Parse(User.FindFirst("id")!.Value);

        [HttpPost]
        [Permission("Category.Create")]
        [EnableRateLimiting("normal")]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            var userId = GetUserId();

            var id = await _service.CreateAsync(dto, userId);

            return Ok(ApiResponse<int>
                .SuccessResponse(id, "Category created successfully"));
        }

        [HttpGet]
        [Permission("Category.Read")]
        [EnableRateLimiting("normal")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();

            return Ok(ApiResponse<List<CategoryListItemDto>>
                .SuccessResponse(data));
        }

        [HttpGet("{id}")]
        [Permission("Category.Read")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
                return NotFound(ApiResponse<string>
                    .Fail("Category not found"));

            return Ok(ApiResponse<CategoryDto>
                .SuccessResponse(data));
        }

        [HttpPut("{id}")]
        [Permission("Category.Update")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> Update(int id, UpdateCategoryDto dto)
        {
            var userId = GetUserId();

            var ok = await _service.UpdateAsync(id, dto, userId);

            if (!ok)
                return NotFound(ApiResponse<string>
                    .Fail("Category not found"));

            return Ok(ApiResponse<string>
                .SuccessResponse("Category updated successfully"));
        }

        [HttpDelete("{id}")]
        [Permission("Category.Delete")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            var ok = await _service.DeleteAsync(id, userId);

            if (!ok)
                return NotFound(ApiResponse<string>
                    .Fail("Category not found"));

            return Ok(ApiResponse<string>
                .SuccessResponse("Category deleted successfully"));
        }
    }
}