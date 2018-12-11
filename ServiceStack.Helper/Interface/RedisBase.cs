using ServiceStack.Helper.Init;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStack.Helper.Interface
{
    /// <summary>
    /// Redis操作基类，继承自IDisposable，用于释放内存
    /// </summary>
    public class RedisBase:IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public IRedisClient _IClient { get; private set; }

        /// <summary>
        /// 构造时完成链接打开
        /// </summary>
        public RedisBase()
        {
            _IClient = RedisManager.GetClient();
        }

        /// <summary>
        /// 释放状态
        /// </summary>
        private bool _dispose = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this._dispose)
            {
                if (disposing)
                {
                    _IClient.Dispose();
                    _IClient = null;
                }
            }
            this._dispose = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 清除所有数据
        /// </summary>
        public virtual void FlushAll()
        {
            _IClient.FlushAll();
        }

        /// <summary>
        /// 保存数据DB至硬盘
        /// </summary>
        public virtual void Save()
        {
            _IClient.Save();
        }

        /// <summary>
        /// 异步保存数据到硬盘
        /// </summary>
        public virtual void SaveAsync()
        {
            _IClient.SaveAsync();
        }
    }
}
