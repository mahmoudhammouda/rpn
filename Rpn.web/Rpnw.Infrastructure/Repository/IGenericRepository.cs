using Rpnw.CrossCutting;
using Rpnw.Infrastructure.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Infrastructure.Repository
{
    public interface IGenericRepository<T> where T : ModelEntity
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        int Add(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        IEnumerable<T> FindByCriteria(object criteria);
        PageResult<T> FindByCriteria(object criteria, int pageIndex, int pageSize);
        PageResult<T> GetAll(int pageIndex, int pageSize);

    }
}
