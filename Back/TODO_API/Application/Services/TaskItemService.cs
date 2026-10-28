using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Domain.Repository_interfaces;
using Domain.Repository_interfaces.Domain.Interfaces;

namespace Application.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly IMapper _mapper;
        private readonly ITaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;

        public TaskItemService(IMapper mapper, ITaskRepository taskRepository, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<PagedResultDto<TaskItemDto>> GetPagedAsync(TaskItemQueryDto query, Guid userId)
        {
            if (query.Page < 1 || query.PageSize < 1 || query.PageSize > 100)
                throw new BadRequestException("Page must be positive and page size must be between 1 and 100.");

            var result = await _taskRepository.GetPagedAsync(userId, query.Search, query.CategoryId, query.Page, query.PageSize);
            return new PagedResultDto<TaskItemDto>(
                _mapper.Map<IReadOnlyList<TaskItemDto>>(result.Items),
                result.TotalCount, query.Page, query.PageSize);
        }

        public async Task<TaskItemDto> GetByIdAsync(Guid id, Guid userId)
        {
            return _mapper.Map<TaskItemDto>(await GetTaskAsync(id, userId));
        }

        public async Task<TaskItemDto> CreateAsync(CreateTaskItemDto dto, Guid userId)
        {
            Validate(dto.Name);
            await ValidateCategoryAsync(dto.CategoryId, userId);
            var task = new TaskItem(dto.Name.Trim(), dto.Description ?? "", dto.DueDate, dto.IsCompleted, dto.CategoryId, userId);
            await _taskRepository.AddAsync(task);
            return _mapper.Map<TaskItemDto>(task);
        }

        public async Task<TaskItemDto> UpdateAsync(Guid id, UpdateTaskItemDto dto, Guid userId)
        {
            Validate(dto.Name);
            await ValidateCategoryAsync(dto.CategoryId, userId);
            var task = await GetTaskAsync(id, userId);
            task.Update(dto.Name.Trim(), dto.Description ?? "", dto.DueDate, dto.IsCompleted, dto.CategoryId);
            await _taskRepository.UpdateAsync(task);
            return _mapper.Map<TaskItemDto>(task);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            await GetTaskAsync(id, userId);
            await _taskRepository.DeleteAsync(id);
        }

        private async Task<TaskItem> GetTaskAsync(Guid id, Guid userId)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task is null || task.UserId != userId)
                throw new NotFoundException($"Task with id {id} not found.");
            return task;
        }

        private async Task ValidateCategoryAsync(Guid id, Guid userId)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null || category.UserId != userId)
                throw new BadRequestException("Select an existing category.");
        }

        private static void Validate(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > 200)
                throw new BadRequestException("Task name must contain 1 to 200 characters.");
        }
    }
}
