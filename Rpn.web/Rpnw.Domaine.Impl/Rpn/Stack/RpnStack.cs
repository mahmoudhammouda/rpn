using Rpnw.Domain.Model.Rpn;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Impl.Rpn.Stack
{
    public class RpnStack : IRpnStack
    {
        private readonly Stack<IRpnElement> _elements;
        public Guid Id { get; }

        public RpnStack()
        {
            _elements = new Stack<IRpnElement>();
            Id = Guid.NewGuid();
        }

        public void Push(IRpnElement element)
        {
            _elements.Push(element);
        }

        public IRpnElement Pop()
        {
            return _elements.Pop();
        }

        public IRpnElement Peek()
        {
            return _elements.Peek();
        }

        public void Clear()
        {
            _elements.Clear();
        }

        public bool IsEmpty => _elements.Count == 0;

        public IEnumerator<IRpnElement> GetEnumerator()
        {
            // Note: To preserve the order, we need to reverse the stack
            return ((IEnumerable<IRpnElement>)_elements.ToArray()).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
