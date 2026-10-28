using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repository_interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> DeleteAsync(Guid id);
    }
}
