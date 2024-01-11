using Rpnw.Domain.Model.Rpn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Impl.Rpn.Stack
{
    public class RpnStackFactory : IRpnStackFactory
    {
        public IRpnStack CreateStack()
        {
            return new RpnStack();
        }
    }
}
