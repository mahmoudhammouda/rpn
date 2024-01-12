using Rpnw.Domain.Model.Rpn;
using Rpnw.Domain.Rpn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Impl.Rpn.Command
{
    public class OperatorCommand : IRpnCommand
    {
        private readonly IRpnOperator _operator;
        private readonly IRpnStack _stack;
        private readonly Stack<IRpnElement> _previousState;
        public IRpnElement Result { get; private set; } 

        public OperatorCommand(IRpnOperator op, IRpnStack stack)
        {
            _operator = op;
            _stack = stack;
            _previousState = new Stack<IRpnElement>();
        }

        public void Execute()
        {
            // Sauvegardez l'état actuel de la stack pour pouvoir annuler l'opération plus tard
            SaveCurrentState();

            // Exécutez l'opération

            _operator.Operate(_stack);

            // Enregistrez le résultat de l'opération
            Result = _operator.GetResult();
        }

        public void Undo()
        {
            // Restaurez l'état de la stack avant l'exécution de l'opérateur
            RestorePreviousState();
        }

        private void SaveCurrentState()
        {
            int operandsNeeded = _operator.GetOperandNumber();
            var enumerator = _stack.GetEnumerator();

            while (operandsNeeded > 0 && enumerator.MoveNext())
            {
                var element = enumerator.Current;
                _previousState.Push(element);
                operandsNeeded--;
            }
        }

        private void RestorePreviousState()
        {
            // Retirer le résultat de la pile
            if (_stack.Peek() == _operator.GetResult() /*&& !Result.IsNull */)
            {
                _stack.Pop();
            }

            // Restaurer les opérandes
            while (_previousState.Count > 0)
            {
                _stack.Push(_previousState.Pop());
            }
        }
    }
}
