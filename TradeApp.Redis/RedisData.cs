using System;
using System.Collections.Generic;
using System.Text;

namespace TradeApp.Redis
{
    [Serializable]
    public class RedisData<T>
    {
        public int ServerId { get; set; }
        public RedisFunction Function { get; set; }
        public Dictionary<string, T> DataList { get; set; }
        public T Data { get; set; }
        public RedisData(int serverId, RedisFunction redisFunction)
        {
            ServerId = serverId;
            Function = redisFunction;
        }
    }
    public enum RedisFunction
    {
        List,
        Add,
        Update,
        Delete
    }
}
