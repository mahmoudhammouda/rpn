using Rpnw.Domain.Impl.Rpn.Operands;
using Rpnw.Domain.Model.Rpn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Impl.Rpn.Operators
{
    public class MulOperator : IRpnOperator
    {
        private IRpnElement _result;

        public void Operate(IRpnStack stack)
        {
            if (stack.Count() < 2)
            {
                throw new InvalidOperationException("Nombre d'operands insuffisant pour l'operation Mul.");
            }

            var right = stack.Pop() as IRpnOperand;
            var left = stack.Pop() as IRpnOperand;
            if (right == null || left == null)
                throw new InvalidOperationException("Nombre d'operands insuffisant pour l'operation Mul.");
            _result = new Operand(left.Value * right.Value);
            stack.Push(_result);
        }

        public int GetOperandNumber()
        {
            return 2; 
        }

        public IRpnElement GetResult()
        {
            return _result;
        }

        public RpnOperatorEnum GetName()
        {
            return RpnOperatorEnum.Mult;
        }


    }
}
