using System;
using System.Configuration;
using ServiceStack.Redis;

namespace RedisDemo.Utils
{
    public static class RedisHelper
    {
        private static readonly IRedisClientsManager Manager;

        static RedisHelper()
        {
            // 读取Redis主机IP配置信息
            string[] redisMasterHosts = ConfigurationManager.AppSettings["RedisServerIP"].Split(',');

            // 如果Redis服务器是主从配置，那么还需要读取Redis Slave机的IP配置信息
            string[] redisSlaveHosts = null;
            var slaveConnection = ConfigurationManager.AppSettings["RedisSlaveServerIP"];
            if (!string.IsNullOrWhiteSpace(slaveConnection))
            {
                redisSlaveHosts = slaveConnection.Split(',');
            }

            // 读取RedisDefaultDb配置
            int defaultDb = 0;
            string defaultDbSetting = ConfigurationManager.AppSettings["RedisDefaultDb"];
            if (!string.IsNullOrWhiteSpace(defaultDbSetting))
            {
                int.TryParse(defaultDbSetting, out defaultDb);
            }
        
            var redisClientManagerConfig = new RedisClientManagerConfig
            {
                MaxReadPoolSize = 50,
                MaxWritePoolSize = 50,
                DefaultDb = defaultDb
            };

            // 创建Redis连接池
            Manager = new PooledRedisClientManager(redisMasterHosts, redisSlaveHosts, redisClientManagerConfig)
            {
                PoolTimeout = 2000,
                ConnectTimeout = 500                
            };
        }

        /// <summary>
        /// 创建Redis连接
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetRedisClient()
        {
            var client = Manager.GetClient();
            return client;
        }
    }
}
