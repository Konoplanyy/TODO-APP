using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Settings;
using AutoMapper;
using Domain.Models;
using Domain.Repository_interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public UserService(
            IMapper mapper,
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IOptions<JwtSettings> jwtSettings)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<UserDto> RegisterAsync(RegisterUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                throw new BadRequestException("Enter a username, email and a password with at least 6 characters.");

            var email = dto.Email.Trim().ToLowerInvariant();
            if (await _userRepository.GetByEmailAsync(email) is not null)
                throw new ConflictException($"User with email {dto.Email} already exists.");

            var user = new User(dto.Username.Trim(), email, _passwordHasher.Hash(dto.Password));

            await _userRepository.AddAsync(user);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email.Trim().ToLowerInvariant())
                ?? throw new UnauthorizedException("Invalid email or password.");

            if (!_passwordHasher.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedException("Invalid email or password.");

            return await IssueAuthResultAsync(user);
        }

        public async Task<AuthResultDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(dto.RefreshToken)
                ?? throw new UnauthorizedException("Invalid refresh token.");

            if (!storedToken.IsActive)
                throw new UnauthorizedException("Refresh token expired or revoked.");

            storedToken.Revoke();
            await _refreshTokenRepository.UpdateAsync(storedToken);

            var user = await _userRepository.GetByIdAsync(storedToken.UserId)
                ?? throw new NotFoundException($"User with id {storedToken.UserId} not found.");

            return await IssueAuthResultAsync(user);
        }

        public async Task LogoutAsync(RefreshTokenDto dto)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(dto.RefreshToken);
            if (storedToken is null)
                return;

            storedToken.Revoke();
            await _refreshTokenRepository.UpdateAsync(storedToken);
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"User with id {id} not found.");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"User with id {id} not found.");

            user.Update(dto.Username, dto.Email);

            await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserDto>(user);
        }

        public async Task ChangePasswordAsync(Guid id, ChangePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"User with id {id} not found.");

            if (!_passwordHasher.Verify(dto.CurrentPassword, user.PasswordHash))
                throw new UnauthorizedException("Current password is incorrect.");

            user.SetPasswordHash(_passwordHasher.Hash(dto.NewPassword));

            await _userRepository.UpdateAsync(user);
        }

        private async Task<AuthResultDto> IssueAuthResultAsync(User user)
        {
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshTokenValue = _tokenService.GenerateRefreshToken();

            var refreshToken = new RefreshToken(
                refreshTokenValue,
                user.Id,
                DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDays));

            await _refreshTokenRepository.AddAsync(refreshToken);

            return new AuthResultDto(accessToken, refreshTokenValue, _mapper.Map<UserDto>(user));
        }
    }
}
