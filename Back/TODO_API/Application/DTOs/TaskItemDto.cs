using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public record TaskItemDto(
        Guid Id,
        string Name,
        string Description,
        DateTime DueDate,
        bool IsCompleted,
        Guid CategoryId,
        DateTime CreatedAt,
        DateTime LastUpdatedAt);

    public record CreateTaskItemDto(string Name, string Description, DateTime DueDate, bool IsCompleted, Guid CategoryId);

    public record UpdateTaskItemDto(string Name, string Description, DateTime DueDate, bool IsCompleted, Guid CategoryId);

    public record TaskItemQueryDto(string? Search = null, Guid? CategoryId = null, int Page = 1, int PageSize = 10);


}
