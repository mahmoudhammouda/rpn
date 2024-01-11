using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Model.Rpn
{
    public class StackNotFoundException : Exception
    {
        public StackNotFoundException(Guid stackId)
            : base($"Aucune stack trouvée avec l'ID {stackId}.") { }
    }
}
