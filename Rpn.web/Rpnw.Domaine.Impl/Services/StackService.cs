using Rpnw.Domain.Enum;
using Rpnw.Domain.Impl.Rpn.Operators;
using Rpnw.Domain.Model.Rpn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Impl.Services
{
    public class StackService : IStackService
    {
        private readonly IRpnCalculator _calculator;

        public StackService(IRpnCalculator calculator)
        {
            _calculator = calculator;
        }

        public Guid CreateStack()
        {
            return _calculator.CreateStack();
        }

        public void DeleteStack(Guid stackId)
        {
             _calculator.RemoveStack(stackId);
        }

        public void ClearStack(Guid stackId)
        {
            _calculator.ClearStack(stackId);
        }

        public IEnumerable<Guid> GetAllStacks()
        {
            // Récupérer tous les identifiants de stacks
            return _calculator.GetAllStackIds();
        }


        public IEnumerable<IRpnElement> GetStackElements(Guid stackId)
        {
            return _calculator.GetAllElements(stackId);
        }

        public void AddOperator(Guid stackId, int operatorId)
        {

            if (!System.Enum.IsDefined(typeof(RpnOperatorEnum), operatorId))
            {
                throw new ArgumentException($"L'identifiant d'opérateur '{operatorId}' est invalide.");
            }

            var operatorEnum = (RpnOperatorEnum)operatorId;
            _calculator.PushOperator(stackId, operatorEnum);
        }

        public void UndoOperator(Guid stackId)
        {
             _calculator.Undo(stackId);
        }

        public void AddOperand(Guid stackId, decimal value)
        {
            _calculator.PushOperand(stackId, value);
        }

    }
}
