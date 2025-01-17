using System;

namespace IconeEsteticaDentaria.CacheManager
{
    public class CacheService
    {
        private MemoryCacheManager cacheManager;
        internal virtual MemoryCacheManager ManagerCache
        {
            get { return cacheManager ?? (cacheManager = new MemoryCacheManager()); }
            set { cacheManager = value; }
        }

        public T ReturnObjectByCache<T>(int usuID, string chaveASerUtilizada, object objeto, bool usarCache = true)
        {
            var chave = String.Format(chaveASerUtilizada, usuID);
            if (!usarCache)
                ManagerCache.Remove(chave);

            return ManagerCache.Get(chave, () =>
            {
                return (T)objeto;
            });
        }
    }
}
