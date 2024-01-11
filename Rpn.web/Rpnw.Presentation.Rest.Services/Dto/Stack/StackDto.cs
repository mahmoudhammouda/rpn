using Rpnw.Domain.Model.Rpn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Presentation.Rest.Services.Dto.Stack
{
    public class StackDto
    {
        public Guid Id { get; set; }
        public IEnumerable<StackElementDto> Elements { get; set; }
    }
}
