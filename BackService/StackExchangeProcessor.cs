﻿using StackExchange.Helper.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackService
{
   public class StackExchangeProcessor
    {
        public static void Show()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string tag = path.Split('/', '\\').Last(s => !string.IsNullOrEmpty(s));
            Console.WriteLine($"这里是 {tag} 启动了。。");
            using (ListService service = new ListService())
            {

                Action act = new Action(() =>
                {
                    while (true)
                    {
                        var result = service.ListRightPop<string>("test");
                        Thread.Sleep(5000);
                        Console.WriteLine($"这里是 {tag} 队列获取的消息 {result}");
                    }
                });
                act.EndInvoke(act.BeginInvoke(null, null));
            }
        }
    }
}
