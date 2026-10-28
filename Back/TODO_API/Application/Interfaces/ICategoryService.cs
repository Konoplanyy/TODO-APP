using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryDto>> GetAllAsync(Guid userId);
        Task<CategoryDto> GetByIdAsync(Guid id, Guid userId);
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto, Guid userId);
        Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto dto, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }

}
