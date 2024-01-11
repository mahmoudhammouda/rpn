using Rpnw.Domain.Impl.Rpn.Command;
using Rpnw.Domain.Impl.Rpn.Operands;
using Rpnw.Domain.Impl.Rpn.Operators;
using Rpnw.Domain.Impl.Rpn.Stack;
using Rpnw.Domain.Model.Rpn;
using Rpnw.Domain.Rpn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Impl.Rpn.Calculator
{
    public class RpnCalculator : IRpnCalculator
    {
        private readonly Dictionary<Guid, IRpnStack> _stacks = new Dictionary<Guid, IRpnStack>();
        private readonly Dictionary<Guid, Stack<IRpnCommand>> _commandHistories = new Dictionary<Guid, Stack<IRpnCommand>>();
        private readonly bool _immediateEvaluation;
        private readonly IRpnStackFactory _stackFactory;
        private readonly IRpnOperatorFactory _operatorFactory = new OperatorFactory();

        public RpnCalculator(IRpnStackFactory stackFactory, bool immediateEvaluation)
        {
            _stackFactory = stackFactory ?? throw new ArgumentNullException(nameof(stackFactory));
            _immediateEvaluation = immediateEvaluation;
        }

        public RpnCalculator(IRpnStackFactory stackFactory)
        {
            _stackFactory = stackFactory ?? throw new ArgumentNullException(nameof(stackFactory));
            _immediateEvaluation = true;
        }

        public Guid CreateStack()
        {
            var id = Guid.NewGuid();
            _stacks[id] = _stackFactory.CreateStack(); ; // Decouplage avec la strucute utilise
            //TODO: faire de meme avec Operand
            _commandHistories[id] = new Stack<IRpnCommand>();
            return id;
        }


        public void PushOperand(Guid stackId, decimal val)
        {
            PushOperand(stackId, new Operand(val));
        }
        private void PushOperand(Guid stackId, IRpnOperand operand)
        {
            EnsureStackExists(stackId, out var stack);
            stack.Push(operand);
        }

        public void PushOperator(Guid stackId, RpnOperatorEnum op)
        {
            PushOperator(stackId, _operatorFactory.CreateOperator(op));
        }

        private void PushOperator(Guid stackId, IRpnOperator op)
        {

            if (EnsureStackExists(stackId, out var stack))
            {
                var command = new OperatorCommand(op, stack);

                if (_immediateEvaluation)
                {
                    // Exécutez immédiatement l'opération et ajoutez-la à l'historique des commandes
                    command.Execute();
                    _commandHistories[stackId].Push(command);
                }
                else
                {
                    // En mode évaluation différée, empilez simplement l'opérateur
                    stack.Push(op);
                }
            }
        }

        public IEnumerable<IRpnElement> GetAllElements(Guid stackId)
        {
            EnsureStackExists(stackId, out var stack);
            return stack.ToArray();

        }

        public IEnumerable<Guid> GetAllStackIds()
        {
            return _stacks.Keys;
        }

        public void RemoveStack(Guid stackId)
        {
            EnsureStackExists(stackId, out var stack);
            _stacks.Remove(stackId);
    
        }

        public void ClearStack(Guid stackId)
        {
            EnsureStackExists(stackId, out var stack);
            stack.Clear();
          
        }

        public void Evaluate(Guid stackId)
        {
            EnsureStackExists(stackId, out var stack);

            var tempStack = new RpnStack();

            foreach (var element in stack.Reverse())
            {

                if (element is IRpnOperator op)
                {
                    // Traiter l'opérateur
                    var command = new OperatorCommand(op, tempStack);
                    command.Execute();
                    _commandHistories[stackId].Push(command);

                }
                else
                {
                    // Si c'est un opérande, le transférer sur la stack temporaire
                    tempStack.Push(element);
                }
            }

            // Nettoyer les éléments de la sack d'origine 
            stack.Clear();

            // Transférer les éléments de tempStack vers stack pour restaurer l'ordre initial
            while (!tempStack.IsEmpty)
            {
                stack.Push(tempStack.Pop());
            }
            
        }

        public IRpnElement Peek(Guid stackId)
        {
            EnsureStackExists(stackId, out var stack);
            return stack.Peek();

        }

        public void Undo(Guid stackId)
        {
            if (_commandHistories.TryGetValue(stackId, out var history) && history.Count > 0)
            {
                var command = history.Pop();
                command.Undo();
            }
        }

        public bool CanUndo(Guid stackId)
        {
            return _commandHistories.TryGetValue(stackId, out var history) && history.Count > 0;
        }

        private bool EnsureStackExists(Guid stackId, out IRpnStack stack)
        {

            if (_stacks.TryGetValue(stackId, out var tmpStack))
            {
                stack = tmpStack;
                return true;
            }
            else
            {
                tmpStack = null;
                throw new StackNotFoundException(stackId);
            }
        }
    }
}
