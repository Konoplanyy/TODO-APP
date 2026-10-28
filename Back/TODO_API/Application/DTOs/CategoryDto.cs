using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public record CategoryDto(Guid Id, string Name, string Color, DateTime CreatedAt, DateTime LastUpdatedAt);

    public record CreateCategoryDto(string Name, string Color);

    public record UpdateCategoryDto(string Name, string Color);


}
