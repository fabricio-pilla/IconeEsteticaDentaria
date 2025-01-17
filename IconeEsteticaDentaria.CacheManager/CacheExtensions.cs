using System;
using System.Web.Configuration;

namespace IconeEsteticaDentaria.CacheManager
{
    public static class CacheExtensions
    {
        public static T Get<T>(this ICache cacheManager, string key, Func<T> acquire)
        {
            //O tempo do cache é definido em minutos
            int tempoExpiracao = 20;
            var cacheTime = WebConfigurationManager.AppSettings["CacheTime"];
            Int32.TryParse(cacheTime, out tempoExpiracao);

            return Get(cacheManager, key, tempoExpiracao, acquire);
        }

        public static T Get<T>(this ICache cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }
            else
            {
                var result = acquire();
                cacheManager.Set(key, result, cacheTime);
                return result;
            }
        }
    }
}
