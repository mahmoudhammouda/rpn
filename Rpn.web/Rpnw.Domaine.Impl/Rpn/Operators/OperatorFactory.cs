using Rpnw.Domain.Model.Rpn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Impl.Rpn.Operators
{
    public class OperatorFactory : IRpnOperatorFactory
    {
        public IRpnOperator CreateOperator(RpnOperatorEnum operation)
        {
            switch (operation)
            {
                case RpnOperatorEnum.Add:
                    return new AddOperator();
                case RpnOperatorEnum.Minus:
                    return new MinusOperator();
                case RpnOperatorEnum.Mult:
                    return new MulOperator();
                case RpnOperatorEnum.Div:
                    return new DivOperator();
                case RpnOperatorEnum.Sqrt:
                    return new SqrtOperator();
                default:
                    throw new ArgumentException("Operation non supportée.");
            }
        }
    }
}
