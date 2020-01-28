using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace TradeApp.Redis
{
    public class RedisCache : IDisposable
    {
        public IDatabase RedisDb;
        public RedisCache(int dbNum)
        {
            if (RedisConnectionFactory.Connection == null) return;
            RedisDb = RedisConnectionFactory.Connection.GetDatabase(dbNum);
            RedisConnectionFactory.Connection.ConnectionFailed += RedisConnectionFailed;
        }
        private void RedisConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            RedisDb = RedisConnectionFactory.Connection.GetDatabase();
        }
        public HashEntry[] HashGetAll(string redisKey)
        {
            if (RedisConnectionFactory.Connection != null && RedisConnectionFactory.Connection.IsConnected)
            {
                return RedisDb.HashGetAll(redisKey);
            }
            return null;
        }
        public void WriteAll<T>(string key, IDictionary<string, T> dictionary)
        {
            if (RedisConnectionFactory.Connection != null && RedisConnectionFactory.Connection.IsConnected)
            {
                RedisDb.HashSet(key,
                    dictionary.Select(i =>
                         new HashEntry(i.Key, JsonConvert.SerializeObject(i.Value))).ToArray(), CommandFlags.FireAndForget);
            }
        }
        public long HashLength(string key)
        {
            if (RedisConnectionFactory.Connection != null && RedisConnectionFactory.Connection.IsConnected)
            {
                return RedisDb.HashLength(key);
            }
            return 0;
        }
        public void WriteAndUpdate<T>(string redisKey, string key, T value)
        {
            if (RedisConnectionFactory.Connection != null && RedisConnectionFactory.Connection.IsConnected)
            {
                RedisDb.HashSet(redisKey, key, JsonConvert.SerializeObject(value));
            }
        }
        public void Delete(string redisKey, string key)
        {
            if (RedisConnectionFactory.Connection != null && RedisConnectionFactory.Connection.IsConnected)
            {
                RedisDb.HashDelete(redisKey, key);
            }
        }
        public void DeleteKey(string key)
        {
            if (RedisConnectionFactory.Connection == null || !RedisConnectionFactory.Connection.IsConnected) return;
            if (RedisDb.KeyExists(key))
            {
                RedisDb.KeyDelete(key);
            }
        }
        public void Dispose()
        {
            RedisConnectionFactory.Connection.Dispose();
        }
    }
}
