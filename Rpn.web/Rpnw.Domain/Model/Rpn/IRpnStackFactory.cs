using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Model.Rpn
{
    public interface IRpnStackFactory
    {
        IRpnStack CreateStack();
    }
}
