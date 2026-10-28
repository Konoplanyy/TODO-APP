using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterUserDto dto);
        Task<AuthResultDto> LoginAsync(LoginDto dto);
        Task<AuthResultDto> RefreshTokenAsync(RefreshTokenDto dto);
        Task LogoutAsync(RefreshTokenDto dto);
        Task<UserDto> GetByIdAsync(Guid id);
        Task<UserDto> UpdateAsync(Guid id, UpdateUserDto dto);
        Task ChangePasswordAsync(Guid id, ChangePasswordDto dto);
    }
}
