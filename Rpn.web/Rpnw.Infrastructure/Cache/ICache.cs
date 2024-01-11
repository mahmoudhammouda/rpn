using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Infrastructure.Cache
{
    public interface ICache
    {
        T Get<T>(string key);
        bool TryGetValue<T>(string key, out T value);
        void Set<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null);
        bool Remove(string key);
    }
}
