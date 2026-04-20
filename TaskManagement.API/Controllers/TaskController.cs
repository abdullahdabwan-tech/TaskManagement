using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskManagement.Application.Common;
using TaskManagement.Application.Common.Query;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Security.Permissions;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private int GetUserId()
            => int.Parse(User.FindFirst("UserId")!.Value);

        [HttpPost]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> Create(CreateTaskDto dto)
        {
            var id = await _taskService.CreateAsync(dto, GetUserId());

            return Ok(ApiResponse<int>.SuccessResponse(id, "Task created successfully"));
        }

        [HttpGet("{id}")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(id);

            if (task == null)
                return NotFound(ApiResponse<string>.Fail("Task not found"));

            return Ok(ApiResponse<object>.SuccessResponse(task));
        }

        [HttpGet]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskService.GetAllAsync(GetUserId());

            return Ok(ApiResponse<object>.SuccessResponse(tasks));
        }

        [HttpPut("{id}")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> Update(int id, UpdateTaskDto dto)
        {
            var result = await _taskService.UpdateAsync(id, dto, GetUserId());

            if (!result)
                return NotFound(ApiResponse<string>.Fail("Task not found"));

            return Ok(ApiResponse<string>.SuccessResponse("Updated successfully"));
        }

        [HttpDelete("{id}")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskService.DeleteAsync(id, GetUserId());

            if (!result)
                return NotFound(ApiResponse<string>.Fail("Task not found"));

            return Ok(ApiResponse<string>.SuccessResponse("Deleted successfully"));
        }

        [HttpGet("paged")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> GetPaged([FromQuery] TaskQueryDto query)
        {
            var result = await _taskService.GetPagedAsync(query);

            return Ok(ApiResponse<object>.SuccessResponse(result));
        }
    }
}
