using System;
using StackExchange.Redis;

namespace TradeApp.Redis
{
    public class RedisConnectionFactory
    {
        //avoid writing static factory classes

        static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            ConfigurationOptions config = new ConfigurationOptions();
            config.EndPoints.Add("10.35.0.49:6379"); //please use IOptions interface for configurations
            config.Password = "g3QF4YpSVB4b8tLS"; //don't hardcode passwords
            //config.PreserveAsyncOrder = false;
            config.AllowAdmin = true;
            ConnectionMultiplexer connectionMultiplexer;
            try
            {
                connectionMultiplexer = ConnectionMultiplexer.Connect(config);
            }
            catch (Exception ex)
            {
                connectionMultiplexer = null;
            }
            return connectionMultiplexer;
        });
        public static ConnectionMultiplexer Connection
        {
            get { return lazyConnection.Value; }
        }
        static RedisConnectionFactory()
        {
        }
        public static void DisposeConnection()
        {
            if (lazyConnection.Value.IsConnected)
                lazyConnection.Value.Dispose();
        }
    }
}
