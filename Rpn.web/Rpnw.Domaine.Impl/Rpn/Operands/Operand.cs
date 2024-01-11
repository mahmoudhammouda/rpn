using Rpnw.Domain.Model.Rpn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Impl.Rpn.Operands
{
    public class Operand : IRpnOperand
    {
        public decimal Value { get; }

        public Operand(decimal value)
        {
            Value = value;
        }
    }
}
