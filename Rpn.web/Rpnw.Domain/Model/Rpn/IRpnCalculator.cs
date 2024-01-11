using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Model.Rpn
{
    public interface IRpnCalculator
    {
        void PushOperand(Guid stackId, decimal val);
        public void PushOperator(Guid stackId, RpnOperatorEnum op);
        void Evaluate(Guid stackId);
        IRpnElement Peek(Guid stackId);
        bool CanUndo(Guid stackId);
        void Undo(Guid stackId);
        Guid CreateStack(); // Crée une nouvelle pile et renvoie son ID, id calculé en memoire

        IEnumerable<IRpnElement> GetAllElements(Guid stackId);
        IEnumerable<Guid> GetAllStackIds();
        void RemoveStack(Guid stackId);
        void ClearStack(Guid stackId);


    }
}
