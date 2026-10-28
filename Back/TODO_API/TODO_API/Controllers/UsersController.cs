using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TODO_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            return Ok(await _userService.RegisterAsync(dto));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            return Ok(await _userService.LoginAsync(dto));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDto dto)
        {
            return Ok(await _userService.RefreshTokenAsync(dto));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenDto dto)
        {
            await _userService.LogoutAsync(dto);
            return NoContent();
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id != Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)) return Forbid();
            return Ok(await _userService.GetByIdAsync(id));
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
        {
            if (id != Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)) return Forbid();
            return Ok(await _userService.UpdateAsync(id, dto));
        }

        [Authorize]
        [HttpPost("{id:guid}/change-password")]
        public async Task<IActionResult> ChangePassword(Guid id, ChangePasswordDto dto)
        {
            if (id != Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)) return Forbid();
            await _userService.ChangePasswordAsync(id, dto);
            return NoContent();
        }
    }
}
