using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace IconeEsteticaDentaria.CacheManager
{
    public class MemoryCacheManager : ICache
    {
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        public T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key, data), policy);
        }

        public virtual bool IsSet(string key)
        {
            return (Cache.Contains(key));

        }

        public virtual void Remove(string key)
        {
            Cache.Remove(key);
        }

        public virtual void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<String>();

            foreach (var item in Cache)
                if (regex.IsMatch(item.Key))
                    keysToRemove.Add(item.Key);

            foreach (string key in keysToRemove)
            {
                Remove(key);
            }
        }

        public virtual void Clear()
        {
            foreach (var item in Cache)
            {
                Remove(item.Key);
            }
        }

        public void ClearKeyByUser(int user)
        {
            var itemUser = Cache.Where(x => x.Key.Contains("_" + user.ToString())).FirstOrDefault();

            if (itemUser.Key != null)
            {
                Remove(itemUser.Key);
            }
        }

        public void VerificaCacheDoUsuario(int idUsuario)
        {
            if (!Cache.Any(x => x.Key == string.Format(CacheKeys.USUARIO_LOGADO_USUARIOKEY, idUsuario)))
            {
                RemoveByPattern(idUsuario.ToString());
            }
        }
    }
}
