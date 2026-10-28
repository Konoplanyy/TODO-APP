using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TODO_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskItemsController : ControllerBase
    {
        private readonly ITaskItemService _taskItemService;

        public TaskItemsController(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] TaskItemQueryDto query)
        {
            return Ok(await _taskItemService.GetPagedAsync(query, UserId));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _taskItemService.GetByIdAsync(id, UserId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskItemDto dto)
        {
            var result = await _taskItemService.CreateAsync(dto, UserId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateTaskItemDto dto)
        {
            return Ok(await _taskItemService.UpdateAsync(id, dto, UserId));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _taskItemService.DeleteAsync(id, UserId);
            return NoContent();
        }

        private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
