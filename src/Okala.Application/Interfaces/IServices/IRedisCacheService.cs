using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okala.Domain.Redis
{
    public interface IRedisCacheService
    {
        Task SetCacheValueAsync<T>(string key, T value, TimeSpan expiration);
        Task<T> GetCacheValueAsync<T>(string key);
    }
}
