using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Model.Rpn
{
    public class OperatorNotFoundException : Exception
    {
        public OperatorNotFoundException(int id)
            : base($"Aucun Operator trouvé avec l'ID {id}.") { }
    }
}
