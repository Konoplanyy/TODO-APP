using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repository_interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task AddAsync(RefreshToken refreshToken);
        Task UpdateAsync(RefreshToken refreshToken);
    }
}
