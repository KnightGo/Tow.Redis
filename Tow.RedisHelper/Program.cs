using StackExchange.Helper.Comm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tow.RedisHelper
{
    class Program
    {
        static void Main(string[] args)
        {

            var s = ConfigurationHelper.ConnectionStrings["RedisHostConnection"];
            try
            {
                //ServiceStack.Redis
                ServiceStackTest.Show();

                //StackExchange
                //StackExchangeTest.Show();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}
