using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repository_interfaces
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<TaskItem> Items, int TotalCount)> GetPagedAsync(Guid userId, string? search, Guid? categoryId, int page, int pageSize);
        Task AddAsync(TaskItem task);
        Task UpdateAsync(TaskItem task);
        Task DeleteAsync(Guid id);
    }
}
