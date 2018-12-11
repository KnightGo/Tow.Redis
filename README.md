# Tow.Redis 封装StackService.Redis和StackExChange.Redis

Redis学习笔记
Redis：Remote Dictionary Server 远程字典（内存）服务器
特点：
基于内存管理、实现了多种数据结构、一个单线程模型的应用程序（原子性操作）、对外提供插入-查询-固化-集群的功能。

一、内存管理：数据可能会丢失、只能做缓存而不能存储、所以代替不了数据库，redis有两种固话到硬盘的方式，

1、snapshot 快照，速度快，如果崩溃可能丢失一部分数据，可以配置一些自动保存策略：1分钟只要有修改就保存

2、Save（阻塞）（单线程）/bgsave（非阻塞）（子进程）

3、AOF：通过日志恢复数据，任何操作都需要日志，效率低，数据不会丢失


二、多种数据结构： String、Hash、Set、ZSet、List（链表），丰富的数据结构、应用特殊场景


三、单线程模型：线程安全，如果多个线程同时来操作，如果时多线程就得加锁，锁过多会影响效率，Redis只有一个线程可以操作数据，避免操作冲突，保证数据操作的

原子性，Redis每一个命令都是原子性的。

原子性：就是一个不可分割的整体、要么都成功、要么都失败

为什么使用Redis？

1、本地缓存空间有限，也无法共享，所以需要分布式缓存，mamcached/redis

2、NoSql：泛指非关系数据库 web2.0兴起  流量越来越大，数据不再是严格的关系型数据库；nosql不是严格的面向对象式，类型更灵活，通常使用内存管理，速度也非常快

Virtual Memory :Redis会自动把一些冷门数据存储到硬盘，可以存储超过内存的数据

翻译操作Redis的脚本，使用Redis：

ServiceStack.Redis：付费

StackExchange.Redis:免费 每分钟最多只能发送3000个请求

基类方法封装：

1、NuGet查找插件包，安装

2、配置Redis类：字段：Redis链接地址、最大读连接数、最大写连接数、本地缓存到期时间、自动重启、是否记录日志；

3、RedisManager管理类：PooledRedisClientManager 使用静态构造函数（单例）初始化Redis

3、RedisBase类，Redis操作的基类，继承IDisposable，主要用于内存释放

基类方法：Dispose()方法，手动GC

封装清除数据：iClient.FlushAll()；会清除所有数据

封装保存数据到硬盘：IClient.Save();//存储快照，期间不会响应其他操作

封装异步保存数据到硬盘：IClient.SaveAsync();//启动子进程完成存储

4、数据操作方法，实现基类RedisBase：

RedisStringHelper.cs

base.iClient.Set<T>(key, value);//保存数据
  
base.iClient.AppendToValue(key, value);//追加数据
  
base.iClient.Set<T>(key, value, sp);//保存数据 设置过期时间
...
RedisListHelper.cs
...
具体参照ServiceStack.Redis常用数据类型操作方法 对Redis操作进行封装

数据类型结构：

//字符串  

//使用场景：商品超卖

//局限：一个对象需要序列化保存和反序列化读取，再序列化保存，会损耗性能；

String：Key-value

//HashTable节约内存空间

//解决字符串序列化损耗性能的问题；

//使用场景：菜单、角色存储；基本可以替代String类型

Hash：HashID->key1-value1

             ->key2-value2
             
             ->key3-value3
             
             
//一个Key-List<Value>，添加的数据无序，自动去重，数据可以做交、差、并、补操作；
  
//使用场景：记录用户ID，关键词等

Set:key 	->value1

->value2

->value3



//和Set的区别在于，自带排序（正序、倒序），同样去重、交叉并补操作，设置权重排序

//应用场景：排行榜、全局排行榜统计

ZSet:key  ->value1-score

->value2-score

->value3-score


//链表结构：双向链表，一块内存会存储上一块内存的地址和下一块内存的地址，增删改效率较高，查询效率较慢；一个List可以放入20条数据；既可以做栈也可以做队列

//使用场景：1、分页功能：第一页为了效率直接读取Redis 缓存的数据，第二页去数据库查询数据；

List:key1-><-value1-><-value2-><-value3 双向链表

String类型使用场景：商品秒杀

有10件商品，100个人想买，假设并发抢购

A查询有没有余量-有->-就更新

B查询有没有余量-有->-就更新

C查询有没有余量-有->-就更新

问题：可能会售出超量的商品

解决：

1、使用数据库事务，用户体验会卡

2、程序中处理，加锁控制（多服务器会出现问题）

3、Redis 基于Redis原子操作的特性解决

（1）设置String：商品->数量 

（2）判断销售完成状态，使用base.iClient.DecrementValue(key);执行-1原子操作，当10件商品销售完成，商品销售完成，秒杀结束；销售状态更改为false;

List处理分布式缓存解决方案：

//查询链表 先进后出：栈实现

PopItemFormList(“ListKey”);

//队列：生产者-消费者模型

//分布式缓存：多服务器都可以访问，多个生产者，多个消费者，任何产品只被消费一次

利用内存比硬盘读取速度快，把数据放到链表中，使用多个服务访问Redis链表数据进行处理，对硬盘进行读写操作，操作完成返回处理结果，解决数据大并发的问题，节约服务器资源，及处理速度；

使用BlockingPopItemFormList（“ListKey”）阻塞式获取链表数据

1、降低流量高峰（并不是提升处理能力，系统的整体能力不变）

2、解除耦合（任务格式定好，各自演变互不影响）

3、高可用（后台服务升级崩溃，不会影响客户端的响应）

弊端：无法实时反馈后台处理结果，需要业务再请求去服务器获取反馈

//发布订阅：观察者

// 一个数据源，多个接收者，只要订阅就可以收到
