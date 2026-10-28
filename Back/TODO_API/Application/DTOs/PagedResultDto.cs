using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public record PagedResultDto<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize);


}
