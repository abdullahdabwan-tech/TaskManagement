using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Roles;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Security.Permissions;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Permission("Role.Read")]
        [EnableRateLimiting("auth")]

        public async Task<IActionResult> GetAll()
        {
            var data = await _roleService.GetAllAsync();
            return Ok(ApiResponse<object>.SuccessResponse(data));
        }

        [HttpGet("{id}")]
        [Permission("Role.Read")]
        [EnableRateLimiting("normal")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _roleService.GetByIdAsync(id);

            if (data == null)
                return NotFound(ApiResponse<string>.Fail("Role not found"));

            return Ok(ApiResponse<object>.SuccessResponse(data));
        }

        [HttpPost]
        [Permission("Role.Create")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> Create(CreateRoleDto dto)
        {
            await _roleService.CreateAsync(dto);

            return Ok(ApiResponse<string>.SuccessResponse("Role created successfully"));
        }

        [HttpPut("{id}")]
        [Permission("Role.Update")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> Update(int id, UpdateRoleDto dto)
        {
            var result = await _roleService.UpdateAsync(id, dto);

            if (!result)
                return NotFound(ApiResponse<string>.Fail("Role not found"));

            return Ok(ApiResponse<string>.SuccessResponse("Role updated successfully"));
        }

        [HttpDelete("{id}")]
        [Permission("Role.Delete")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roleService.DeleteAsync(id);

            if (!result)
                return NotFound(ApiResponse<string>.Fail("Role not found"));

            return Ok(ApiResponse<string>.SuccessResponse("Role deleted successfully"));
        }

        [HttpPost("assign-permission")]
        [Permission("Role.AssignPermission")]
        [EnableRateLimiting("auth")]

        public async Task<IActionResult> AssignPermission(AssignPermissionDto dto)
        {
            await _roleService.AssignPermissionAsync(dto);

            return Ok(ApiResponse<string>.SuccessResponse("Permission assigned"));
        }

        [HttpPost("remove-permission")]
        [Permission("Role.AssignPermission")]
        [EnableRateLimiting("auth")]

        public async Task<IActionResult> RemovePermission(AssignPermissionDto dto)
        {
            await _roleService.RemovePermissionAsync(dto);

            return Ok(ApiResponse<string>.SuccessResponse("Permission removed"));
        }
    }
}