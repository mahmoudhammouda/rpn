using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Model.Rpn
{
    public interface IRpnStack : IEnumerable<IRpnElement>
    {
        Guid Id { get; }
        void Push(IRpnElement element);
        IRpnElement Pop();
        IRpnElement Peek();
        void Clear();
        bool IsEmpty { get; }
    }
}
