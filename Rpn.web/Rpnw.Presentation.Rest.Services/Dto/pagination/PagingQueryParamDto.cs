using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Presentation.Rest.Services.Dto.pagination
{
    public class PagingQueryParamDto
    {
        public int? PageIndex { get; set; } = 0;
        public int? PageSize { get; set; } = 1;

        public bool HasValue() 
        {
            return PageIndex.HasValue || PageSize.HasValue;
        }
          
    }
}
