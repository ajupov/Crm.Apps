using System;
using ServiceStack.Redis;

namespace Crm.Infrastructure.HotStorage.HotStorage
{
    public class HotStorage : IHotStorage
    {
        private readonly IRedisClientsManager _redisClientsManager;

        public HotStorage(
            IRedisClientsManager redisClientsManager)
        {
            _redisClientsManager = redisClientsManager;
        }

        public void SetValue<T>(
            string key,
            T value,
            TimeSpan timeSpan)
        {
            using (var client = _redisClientsManager.GetClient())
            {
                client.Set(key, value, timeSpan);
            }
        }

        public void IsExist(
            string key)
        {
            using (var client = _redisClientsManager.GetClient())
            {
                client.ContainsKey(key);
            }
        }

        public T GetValue<T>(
            string key)
        {
            using (var client = _redisClientsManager.GetClient())
            {
                return client.Get<T>(key);
            }
        }
    }
}