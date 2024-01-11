using Rpnw.CrossCutting;
using Rpnw.Domain.Model;
using Rpnw.Domain.Model.Rpn;
using Rpnw.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Impl.Services
{
    public class OperatorService : IOperatorService
    {

        public OperatorService()
        {
        }
        public IEnumerable<Operator> GetAllOperators()
        {
            return System.Enum.GetValues(typeof(RpnOperatorEnum))
                       .Cast<RpnOperatorEnum>()
                       .Select(op => new Operator
                       {
                           Id = (int)op,
                           Name = op.ToString(),
                           Description = EnumHelper.GetDescription(op)
                        });
        }

        public Operator GetOperatorById(int id)
        {
            if (System.Enum.IsDefined(typeof(RpnOperatorEnum), id))
            {
                var opEnum = (RpnOperatorEnum)id;
                return new Operator
                {
                    Id = id,
                    Name = opEnum.ToString(),
                    Description = EnumHelper.GetDescription(opEnum)
                };
            }

            throw new OperatorNotFoundException(id);

        }
    }
}
