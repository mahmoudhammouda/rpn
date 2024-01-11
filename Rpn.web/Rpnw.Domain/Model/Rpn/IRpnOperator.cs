using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Model.Rpn
{
    public interface IRpnOperator : IRpnElement
    {
        void Operate(IRpnStack stack);
        int GetOperandNumber(); // Retourne le nombre d'opérandes nécessaires pour l'opération
        IRpnElement GetResult(); // Retourne le résultat de la dernière opération
        RpnOperatorEnum GetName();
    }
}
