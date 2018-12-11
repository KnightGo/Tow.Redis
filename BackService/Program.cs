using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackService
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ServiceStackProcessor.Show();

            }catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            Console.Read();
        }
    }
}
