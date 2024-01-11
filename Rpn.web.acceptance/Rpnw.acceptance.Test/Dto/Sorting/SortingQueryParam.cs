using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.acceptance.Test
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
