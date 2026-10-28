using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Domain.Repository_interfaces.Domain.Interfaces;
using Domain.Repository_interfaces;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITaskRepository _taskRepository;

        public CategoryService(IMapper mapper, ICategoryRepository categoryRepository, ITaskRepository taskRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _taskRepository = taskRepository;
        }

        public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(Guid userId)
        {
            return _mapper.Map<IReadOnlyList<CategoryDto>>(await _categoryRepository.GetAllAsync(userId));
        }

        public async Task<CategoryDto> GetByIdAsync(Guid id, Guid userId)
        {
            return _mapper.Map<CategoryDto>(await GetCategoryAsync(id, userId));
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, Guid userId)
        {
            Validate(dto.Name, dto.Color);
            var category = new Category(dto.Name.Trim(), dto.Color, userId);
            await _categoryRepository.AddAsync(category);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto dto, Guid userId)
        {
            Validate(dto.Name, dto.Color);
            var category = await GetCategoryAsync(id, userId);
            category.Update(dto.Name.Trim(), dto.Color);
            await _categoryRepository.UpdateAsync(category);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            await GetCategoryAsync(id, userId);
            var tasks = await _taskRepository.GetPagedAsync(userId, null, id, 1, 1);
            if (tasks.TotalCount > 0)
                throw new ConflictException("Move or delete the tasks in this category first.");
            await _categoryRepository.DeleteAsync(id);
        }

        private async Task<Category> GetCategoryAsync(Guid id, Guid userId)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null || category.UserId != userId)
                throw new NotFoundException($"Category with id {id} not found.");
            return category;
        }

        private static void Validate(string name, string color)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
                throw new BadRequestException("Category name must contain 1 to 100 characters.");
            if (!System.Text.RegularExpressions.Regex.IsMatch(color ?? "", "^#[0-9a-fA-F]{6}$"))
                throw new BadRequestException("Category color must be a hex color.");
        }
    }
}
