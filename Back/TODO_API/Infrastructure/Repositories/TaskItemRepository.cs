using Domain.Models;
using Domain.Repository_interfaces;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class TaskItemRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaskItem task)
        {
            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await GetByIdAsync(id);
            if (task is null)
                return;

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task<(IReadOnlyList<TaskItem> Items, int TotalCount)> GetPagedAsync(Guid userId, string? search, Guid? categoryId, int page, int pageSize)
        {
            var tasks = _context.TaskItems.AsNoTracking().Where(x => x.UserId == userId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = $"%{search.Trim()}%";
                tasks = tasks.Where(x => EF.Functions.Like(x.Name, term) || EF.Functions.Like(x.Description, term));
            }

            if (categoryId.HasValue)
                tasks = tasks.Where(x => x.CategoryId == categoryId.Value);

            var totalCount = await tasks.CountAsync();
            var items = await tasks.OrderByDescending(x => x.CreatedAt)
                .ThenBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.TaskItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();
        }
    }
}
