using Rpnw.Domain.Model.Rpn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Rpn
{
    public interface IRpnCommand
    {
        void Execute();
        void Undo();
        IRpnElement Result { get; }
    }
}
