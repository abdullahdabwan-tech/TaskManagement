using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Comments;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Security.Permissions;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/comments")]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _service;

        public CommentsController(ICommentsService service)
        {
            _service = service;
        }

        private int GetUserId()
            => int.Parse(User.FindFirst("id")!.Value);

        [HttpPost]
        [Permission("Comment.Create")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> Create(CreateCommentDto dto)
        {
            var userId = GetUserId();

            var id = await _service.CreateAsync(dto, userId);

            return Ok(ApiResponse<int>
                .SuccessResponse(id, "Comment added successfully"));
        }

        [HttpGet("task/{taskId}")]
        [Permission("Comment.Read")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> GetByTask(int taskId)
        {
            var data = await _service.GetByTaskIdAsync(taskId);

            return Ok(ApiResponse<List<CommentDto>>
                .SuccessResponse(data));
        }

        [HttpPut("{id}")]
        [Permission("Comment.Update")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> Update(int id, UpdateCommentDto dto)
        {
            var userId = GetUserId();

            var ok = await _service.UpdateAsync(id, dto, userId);

            if (!ok)
                return NotFound(ApiResponse<string>.Fail("Comment not found"));

            return Ok(ApiResponse<string>
                .SuccessResponse("Comment updated"));
        }

        [HttpDelete("{id}")]
        [Permission("Comment.Delete")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            var ok = await _service.DeleteAsync(id, userId);

            if (!ok)
                return NotFound(ApiResponse<string>.Fail("Comment not found"));

            return Ok(ApiResponse<string>
                .SuccessResponse("Comment deleted"));
        }
    }
}