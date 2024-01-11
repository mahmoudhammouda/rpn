using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Presentation.Rest.Services.Dto.Sorting
{
    public record SortingQueryParam
    {
        public string? SortBy { get; }
        public SortDirectionEnum? SortDirection { get; }

        public bool HasValue()
        {
            return !string.IsNullOrEmpty(SortBy)|| SortDirection.HasValue;
        }

    }
}
