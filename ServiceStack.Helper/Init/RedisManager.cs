using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStack.Helper.Init
{
    /// <summary>
    /// Redis管理中心
    /// </summary>
   public class RedisManager
    {
        /// <summary>
        /// redis配置文件
        /// </summary>
        private static RedisConfigInfo _RedisConfigInfo = new RedisConfigInfo();

        /// <summary>
        /// Redis客户端池化管理
        /// </summary>
        private static PooledRedisClientManager prcManager;

        static RedisManager()
        {
            CreateManager();
        }
        /// <summary>
        /// 创建连接对象
        /// </summary>
        private static void CreateManager()
        {
            string[] WriteServerConStr = _RedisConfigInfo.WriteServerList.Split(',');
            string[] ReadServerConStr = _RedisConfigInfo.ReadServerList.Split(',');
            prcManager = new PooledRedisClientManager(ReadServerConStr, WriteServerConStr, new RedisClientManagerConfig
            {
                MaxReadPoolSize = _RedisConfigInfo.MaxReadPoolSize,
                MaxWritePoolSize = _RedisConfigInfo.MaxReadPoolSize,
                AutoStart = _RedisConfigInfo.AutoStart

            });
        }
        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetClient()
        {
            return prcManager.GetClient();
        }
    }
}
