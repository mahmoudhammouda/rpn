using Rpnw.Domain.Impl.Rpn.Operands;
using Rpnw.Domain.Model.Rpn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Impl.Rpn.Operators
{
    public class SqrtOperator : IRpnOperator
    {
        private IRpnElement _result;

        public void Operate(IRpnStack stack)
        {
            if (stack.Count() < 1)
            {
                throw new InvalidOperationException("Nombre d'operands insuffisant pour l'operation Sqrt.");
            }

            var operand = stack.Pop() as IRpnOperand;

            if (operand == null)
            {
                throw new InvalidOperationException("Operand doit etre Numeric pour Sqrt.");
            }

            if (operand.Value < 0)
            {
                throw new InvalidOperationException("On ne peut pas calculer le Sqrt d'un nombre negatif.");
            }

            _result = new Operand((decimal)Math.Sqrt((double)operand.Value));
            stack.Push(_result);
        }

        public int GetOperandNumber()
        {
            return 1; 
        }

        public IRpnElement GetResult()
        {
            return _result;
        }

        public RpnOperatorEnum GetName()
        {
            return RpnOperatorEnum.Sqrt;
        }
    }
}
