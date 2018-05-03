
using System;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Newtonsoft;
using BaseBallVR.Redis.DB;


namespace BaseBallVR.Redis
{
    public interface ICacheRedis : IBase { }

    public class CacheRedis : ICacheRedis
    {
        public CacheRedis(string configuration)
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(configuration);
            });
        }

        public ICacheClient GetCacheClient()
        {
            return CacheClient;
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        private static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        private static Lazy<StackExchangeRedisCacheClient> lazyCacheClient = new Lazy<StackExchangeRedisCacheClient>(() =>
         {
             var serializer = new NewtonsoftSerializer();

             return new StackExchangeRedisCacheClient(Connection, serializer);
         });

        private static StackExchangeRedisCacheClient CacheClient
        {
            get
            {
                return lazyCacheClient.Value;
            }
        }

    }
}