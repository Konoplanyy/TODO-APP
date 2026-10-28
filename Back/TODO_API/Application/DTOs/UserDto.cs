using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public record UserDto(Guid Id, string Username, string Email, DateTime CreatedAt, DateTime LastUpdatedAt);

    public record RegisterUserDto(string Username, string Email, string Password);

    public record LoginDto(string Email, string Password);

    public record UpdateUserDto(string Username, string Email);

    public record ChangePasswordDto(string CurrentPassword, string NewPassword);

    public record AuthResultDto(string Token, string RefreshToken, UserDto User);


}
