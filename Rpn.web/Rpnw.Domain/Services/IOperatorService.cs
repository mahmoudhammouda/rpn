using Rpnw.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Services
{
    public interface IOperatorService
    {
        IEnumerable<Operator> GetAllOperators();
        Operator GetOperatorById(int id);

    }
}
