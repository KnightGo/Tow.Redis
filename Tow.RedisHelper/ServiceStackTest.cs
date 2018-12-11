using ServiceStack.Helper.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tow.RedisHelper
{
    public class ServiceStackTest
    {
        public static void Show()
        {
            //{//String类型
            //    using (StringService service = new StringService())
            //    {
            //        service.Set("test_String1", "123");
            //        service.Set<int>("test_Int1", 1);
            //        service.Set<bool>("test_String1", true);
            //        service.Set("test_String2", "456", new TimeSpan(0, 0, 0, 5));
            //    };
            //}
            //{//Hash类型
            //    using (HashService service = new HashService())
            //    {
            //        service.FlushAll();
            //        service.SetEntryInHash("student", "id", "123456");
            //        service.SetEntryInHash("student", "name", "张xx");
            //        service.SetEntryInHash("student", "remark", "高级班的学员");

            //        var keys = service.GetHashKeys("student");
            //        var values = service.GetHashValues("student");
            //        var keyValues = service.GetAllEntriesFromHash("student");
            //        Console.WriteLine(service.GetValueFromHash("student", "id"));

            //        service.SetEntryInHashIfNotExists("student", "name", "太子爷");
            //        service.SetEntryInHashIfNotExists("student", "description", "高级班的学员2");

            //        Console.WriteLine(service.GetValueFromHash("student", "name"));
            //        Console.WriteLine(service.GetValueFromHash("student", "description"));
            //        service.RemoveEntryFromHash("student", "description");
            //        Console.WriteLine(service.GetValueFromHash("student", "description"));
            //    }
            //}
            //{//Set
            //    using (SetService service = new SetService())
            //    {
            //        service.Add("Set1", "1");
            //        service.Add("Set1", "2");
            //        service.Add("Set1", "3");
            //        service.Add("Set1", "4");

            //        service.Add("Set2", "2");
            //        service.Add("Set2", "3");
            //        service.Add("Set2", "4");
            //        service.Add("Set2", "45");
            //    }

            //}

            //{//ZSet
            //    using (ZSetService service = new ZSetService())
            //    {
            //        service.Add("Set1", "1");
            //        service.Add("Set1", "2");
            //        service.Add("Set1", "3");
            //        service.Add("Set1", "4");

            //        service.Add("Set2", "2");
            //        service.Add("Set2", "3");
            //        service.Add("Set2", "4");
            //        service.Add("Set2", "45");

            //        List<string> sList1=service.GetAllDesc("Set1");
            //        List<string> sList2 = service.GetAllDesc("Set2");
            //    }

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
                                service.Get(key).ForEach(s => { Console.WriteLine("data:" + s); });
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
                                    service.LPush(key, value);
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
                                    service.FlushAll();
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
