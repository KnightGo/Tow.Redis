using ServiceStack.Helper.Interface;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStack.Helper.Service
{
    /// <summary>
    ///  Redis list的实现为一个双向链表，即可以支持反向查找和遍历，更方便操作，不过带来了部分额外的内存开销，
    ///  Redis内部的很多实现，包括发送缓冲队列等也都是用的这个数据结构。  
    /// </summary>
    public class ListService:RedisBase
    {
        #region 赋值
        /// <summary>
        /// 从左侧向list中添加值
        /// </summary>
        public void LPush(string key, string value)
        {
            base._IClient.PushItemToList(key, value);
        }

        public object BlockingPopItemFromLists(string[] v, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从左侧向list中添加值，并设置过期时间
        /// </summary>
        public void LPush(string key, string value, DateTime dt)
        {

            base._IClient.PushItemToList(key, value);
            base._IClient.ExpireEntryAt(key, dt);
        }
        /// <summary>
        /// 从左侧向list中添加值，设置过期时间
        /// </summary>
        public void LPush(string key, string value, TimeSpan sp)
        {
            base._IClient.PushItemToList(key, value);
            base._IClient.ExpireEntryIn(key, sp);
        }
        /// <summary>
        /// 从右侧向list中添加值
        /// </summary>
        public void RPush(string key, string value)
        {
            base._IClient.PrependItemToList(key, value);
        }
        /// <summary>
        /// 从右侧向list中添加值，并设置过期时间
        /// </summary>    
        public void RPush(string key, string value, DateTime dt)
        {
            base._IClient.PrependItemToList(key, value);
            base._IClient.ExpireEntryAt(key, dt);
        }
        /// <summary>
        /// 从右侧向list中添加值，并设置过期时间
        /// </summary>        
        public void RPush(string key, string value, TimeSpan sp)
        {
            base._IClient.PrependItemToList(key, value);
            base._IClient.ExpireEntryIn(key, sp);
        }
        /// <summary>
        /// 添加key/value
        /// </summary>     
        public void Add(string key, string value)
        {
            base._IClient.AddItemToList(key, value);
        }
        /// <summary>
        /// 添加key/value ,并设置过期时间
        /// </summary>  
        public void Add(string key, string value, DateTime dt)
        {
            base._IClient.AddItemToList(key, value);
            base._IClient.ExpireEntryAt(key, dt);
        }
        /// <summary>
        /// 添加key/value。并添加过期时间
        /// </summary>  
        public void Add(string key, string value, TimeSpan sp)
        {
            base._IClient.AddItemToList(key, value);
            base._IClient.ExpireEntryIn(key, sp);
        }
        /// <summary>
        /// 为key添加多个值
        /// </summary>  
        public void Add(string key, List<string> values)
        {
            base._IClient.AddRangeToList(key, values);
        }
        /// <summary>
        /// 为key添加多个值，并设置过期时间
        /// </summary>  
        public void Add(string key, List<string> values, DateTime dt)
        {
            base._IClient.AddRangeToList(key, values);
            base._IClient.ExpireEntryAt(key, dt);
        }
        /// <summary>
        /// 为key添加多个值，并设置过期时间
        /// </summary>  
        public void Add(string key, List<string> values, TimeSpan sp)
        {
            base._IClient.AddRangeToList(key, values);
            base._IClient.ExpireEntryIn(key, sp);
        }
        #endregion

        #region 获取值
        /// <summary>
        /// 获取list中key包含的数据数量
        /// </summary>  
        public long Count(string key)
        {
            return base._IClient.GetListCount(key);
        }
        /// <summary>
        /// 获取key包含的所有数据集合
        /// </summary>  
        public List<string> Get(string key)
        {
            return base._IClient.GetAllItemsFromList(key);
        }
        /// <summary>
        /// 获取key中下标为star到end的值集合 
        /// </summary>  
        public List<string> Get(string key, int star, int end)
        {
            return base._IClient.GetRangeFromList(key, star, end);
        }
        #endregion

        #region 阻塞命令
        /// <summary>
        ///  阻塞命令：从list中keys的尾部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>  
        public string BlockingPopItemFromLists(string key, TimeSpan? sp)
        {
            return base._IClient.BlockingDequeueItemFromList(key, sp);
        }
        /// <summary>
        ///  阻塞命令：从list中keys的尾部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>  
        public ItemRef BlockingPopItemFromLists(string[] keys, TimeSpan? sp)
        {
            return base._IClient.BlockingPopItemFromLists(keys, sp);
        }
        /// <summary>
        ///  阻塞命令：从list中keys的尾部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>  
        public string BlockingDequeueItemFromList(string key, TimeSpan? sp)
        {
            return base._IClient.BlockingDequeueItemFromList(key, sp);
        }
        /// <summary>
        /// 阻塞命令：从list中keys的尾部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>  
        public ItemRef BlockingDequeueItemFromLists(string[] keys, TimeSpan? sp)
        {
            return base._IClient.BlockingDequeueItemFromLists(keys, sp);
        }
        /// <summary>
        /// 阻塞命令：从list中key的头部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>  
        public string BlockingRemoveStartFromList(string keys, TimeSpan? sp)
        {
            return base._IClient.BlockingRemoveStartFromList(keys, sp);
        }
        /// <summary>
        /// 阻塞命令：从list中key的头部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>  
        public ItemRef BlockingRemoveStartFromLists(string[] keys, TimeSpan? sp)
        {
            return base._IClient.BlockingRemoveStartFromLists(keys, sp);
        }
        /// <summary>
        /// 阻塞命令：从list中一个fromkey的尾部移除一个值，添加到另外一个tokey的头部，并返回移除的值，阻塞时间为sp
        /// </summary>  
        public string BlockingPopAndPushItemBetweenLists(string fromkey, string tokey, TimeSpan? sp)
        {
            return base._IClient.BlockingPopAndPushItemBetweenLists(fromkey, tokey, sp);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 从尾部移除数据，返回移除的数据
        /// </summary>  
        public string PopItemFromList(string key)
        {
            return base._IClient.PopItemFromList(key);
        }
        /// <summary>
        /// 移除list中，key/value,与参数相同的值，并返回移除的数量
        /// </summary>  
        public long RemoveItemFromList(string key, string value)
        {
            return base._IClient.RemoveItemFromList(key, value);
        }
        /// <summary>
        /// 从list的尾部移除一个数据，返回移除的数据
        /// </summary>  
        public string RemoveEndFromList(string key)
        {
            return base._IClient.RemoveEndFromList(key);
        }
        /// <summary>
        /// 从list的头部移除一个数据，返回移除的值
        /// </summary>  
        public string RemoveStartFromList(string key)
        {
            return base._IClient.RemoveStartFromList(key);
        }
        /// <summary>
        /// 删除指定key中的所有list
        /// </summary>
        /// <param name="key"></param>
        /// <returns>移除数量</returns>
        public long RemoveAllByKey(string key)
        {
            long length=Count(key);
            
            for (long i = length; i > 0; i--)
            {

                PopItemFromList(key);
                
            }
            return length;
        }
        #endregion

        #region 其它
        /// <summary>
        /// 从一个list的尾部移除一个数据，添加到另外一个list的头部，并返回移动的值
        /// </summary>  
        public string PopAndPushItemBetweenLists(string fromKey, string toKey)
        {
            return base._IClient.PopAndPushItemBetweenLists(fromKey, toKey);
        }
        #endregion
    }
}
