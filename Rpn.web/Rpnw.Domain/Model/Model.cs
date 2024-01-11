using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Model
{
    public class Model
    {
        public int Id { get; private set; }
        public Model(int id)
        {
            Id = id;
        }

        public void SetAndValidateId(int val)
        {
            if (val < 0)
                throw new ArgumentException("ID can not be negative.");

            Id = val;
        }
    }
}
