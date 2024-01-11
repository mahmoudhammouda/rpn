using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Model.Rpn
{
    public enum RpnOperatorEnum
    {
        [Description("Addition operation")]
        Add = 1,
        [Description("Subtraction operation")]
        Minus = 2,
        [Description("Multiplication operation")]
        Mult = 3,
        [Description("Division operation")]
        Div = 4,
        [Description("Square root operation")]
        Sqrt = 5
    }
}
