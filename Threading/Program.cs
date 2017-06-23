using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Threading
{
    class Program
    {
        public static ThreadLocal<int> _field = new ThreadLocal<int>(()=>
        {
            return Thread.CurrentThread.ManagedThreadId;
        });

        static void Main(string[] args)
        {
            new Thread(()=> 
            {
                for (int i = 0; i < _field.Value; i++)
                {
                    Console.WriteLine("Thread A: {0}", i);
                }
            }).Start();

            new Thread(() =>
            {
                for (int i = 0; i < _field.Value; i++)
                {
                    Console.WriteLine("Thread B: {0}", i);
                }
            }).Start();
            
            Console.ReadKey();
            //Thread t = new Thread(new ParameterizedThreadStart(MyThread));
            //t.IsBackground = false;
            //t.Start(5);
        }

        public static void MyThread(object x)
        {
            for (int i = 0; i < (int)x; i++)
            {
                Console.WriteLine("My Thread: {0}", i);
                Thread.Sleep(1000);
            }
        }
    }
}
