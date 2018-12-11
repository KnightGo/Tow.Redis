using StackExchange.Helper.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tow.RedisHelper
{
    public class StackExchangeTest
    {
      public static void Show()
        {
            //{//String
            //    using (StringService service = new StringService())
            //    {
            //        service.StringSet("ExStr", "1");
            //        service.StringSet("ExStr", "2");
            //        service.StringSet("ExStr", "3");
            //        service.StringSet("ExStr", "4");

            //    };
            //}

            {//List（从控制台写入数据，BackService开启线程执行并发处理）
                using (ListService service = new ListService())
                {
                    Action act = new Action(() =>
                    {
                        while (true)
                        {
                            Console.WriteLine("请输入:r:读取,w:写入,c:清空");
                            string testTask = Console.ReadLine();
                            if (testTask == "r")
                            {
                                Console.WriteLine("请输入Key（-1退出）：");
                                string key = Console.ReadLine();
                                if (key == "-1")
                                    break;
                                service.ListRange<String>(key).ForEach(s => { Console.WriteLine("data:" + s); });
                            }
                            else if (testTask == "w")
                            {
                                Console.WriteLine("请输入Key（-1退出）：");
                                string key = Console.ReadLine();
                                while (true && key != "-1")
                                {
                                    Console.WriteLine("请输入Value（-1退出）：");
                                    string value = Console.ReadLine();
                                    if (value == "-1")
                                    {
                                        break;
                                    }
                                    service.ListRightPush(key, value);
                                }
                            }
                            else if (testTask == "c")
                            {
                                Console.WriteLine("请输入Key（-1退出/-all全部清空）：");
                                string key = Console.ReadLine();
                                if (key == "-1")
                                {
                                    break;
                                }
                                else if (key == "-all")
                                {
                                    service.KeyFulsh();
                                }
                                else
                                {
                                    service.RemoveAllByKey(key);
                                }
                            }
                            else
                            {
                                Console.WriteLine("请重新输入");
                            }
                        }
                    });
                    //通过EndInvoke方法检测异步调用的结果。如果异步调用尚未完成，EndInvoke将阻塞调用线程，直到它完成
                    act.EndInvoke(act.BeginInvoke(null, null));

                }
            }
        }

    }
}

