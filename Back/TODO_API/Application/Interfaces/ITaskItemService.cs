using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ITaskItemService
    {
        Task<PagedResultDto<TaskItemDto>> GetPagedAsync(TaskItemQueryDto query, Guid userId);
        Task<TaskItemDto> GetByIdAsync(Guid id, Guid userId);
        Task<TaskItemDto> CreateAsync(CreateTaskItemDto dto, Guid userId);
        Task<TaskItemDto> UpdateAsync(Guid id, UpdateTaskItemDto dto, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }
}
