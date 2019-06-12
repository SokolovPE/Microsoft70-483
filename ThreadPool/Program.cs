using System;
using System.Threading;

namespace ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem((s)=>{
                System.Console.WriteLine("Working on a thread from threadpool");
            });
            Console.ReadLine();
        }
    }
}
