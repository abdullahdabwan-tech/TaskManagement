using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Security.Permissions;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _service;

        public UsersController(IUsersService service)
        {
            _service = service;
        }

        [HttpPost]
        [Permission("User.Create")]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            var id = await _service.CreateAsync(dto);

            return Ok(ApiResponse<int>.SuccessResponse(id, "User created successfully"));
        }

        [HttpGet]
        [Permission("User.Read")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();

            return Ok(ApiResponse<List<UserListItemDto>>.SuccessResponse(data));
        }

        [HttpGet("{id}")]
        [Permission("User.Read")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
                return NotFound(ApiResponse<string>.Fail("User not found"));

            return Ok(ApiResponse<UserDto>.SuccessResponse(data));
        }

        [HttpPut("{id}")]
        [Permission("User.Update")]
        public async Task<IActionResult> Update(int id, UpdateUserDto dto)
        {
            var ok = await _service.UpdateAsync(id, dto);

            if (!ok)
                return NotFound(ApiResponse<string>.Fail("User not found"));

            return Ok(ApiResponse<string>.SuccessResponse("User updated successfully"));
        }

        [HttpDelete("{id}")]
        [Permission("User.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);

            if (!ok)
                return NotFound(ApiResponse<string>.Fail("User not found"));

            return Ok(ApiResponse<string>.SuccessResponse("User deleted successfully"));
        }
    }
}
