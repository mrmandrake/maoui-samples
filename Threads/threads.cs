using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Threads
{
    public class Program
    {
        public static void Main(String[] args)
        {
            Console.WriteLine("Multithread example....");

            var task1 = new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("T1");
                    Thread.Sleep(777);
                }
            });

            var task2 = new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("X2");
                    Thread.Sleep(888);
                }

            });

            task1.Start();
            task2.Start();
            Console.WriteLine("Hello world");
        }
    }
}
