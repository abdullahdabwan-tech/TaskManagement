using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Reactions;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Security.Permissions;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/reactions")]
    public class ReactionsController : ControllerBase
    {
        private readonly IReactionsService _service;

        public ReactionsController(IReactionsService service)
        {
            _service = service;
        }

        private int GetUserId()
            => int.Parse(User.FindFirst("id")!.Value);

        [HttpPost]
        [Permission("Reaction.Create")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> Add(CreateReactionDto dto)
        {
            var userId = GetUserId();

            var id = await _service.CreateAsync(dto, userId);

            return Ok(ApiResponse<int>
                .SuccessResponse(id, "Reaction added"));
        }

        [HttpDelete("{id}")]
        [Permission("Reaction.Delete")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> Remove(int id)
        {
            var userId = GetUserId();

            var ok = await _service.DeleteAsync(id, userId);

            if (!ok)
                return NotFound(ApiResponse<string>
                    .Fail("Reaction not found"));

            return Ok(ApiResponse<string>
                .SuccessResponse("Reaction removed"));
        }

        [HttpGet("task/{taskId}")]
        [Permission("Reaction.Read")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> GetByTask(int taskId)
        {
            var data = await _service.GetByTaskIdAsync(taskId);

            return Ok(ApiResponse<List<ReactionDto>>
                .SuccessResponse(data));
        }

        [HttpGet("comment/{commentId}")]
        [Permission("Reaction.Read")]
        [EnableRateLimiting("normal")]

        public async Task<IActionResult> GetByComment(int commentId)
        {
            var data = await _service.GetByCommentIdAsync(commentId);

            return Ok(ApiResponse<List<ReactionDto>>
                .SuccessResponse(data));
        }
    }
}