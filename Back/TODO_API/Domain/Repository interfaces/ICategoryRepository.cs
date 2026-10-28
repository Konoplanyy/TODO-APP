using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repository_interfaces
{
    namespace Domain.Interfaces
    {
        public interface ICategoryRepository
        {
            Task<Category?> GetByIdAsync(Guid id);
            Task<IEnumerable<Category>> GetAllAsync(Guid userId);
            Task AddAsync(Category category);
            Task UpdateAsync(Category category);
            Task DeleteAsync(Guid id);
        }
    }
}
