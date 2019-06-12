using System;
using System.Threading;

namespace ThreadPools
{
    class Program
    {
        // Thread Pool is created to reuse threads you're finished to use
        // Instead of letting thread die, you send it back to the pool
        // where it can be reused

        // Thread pool ensures that each request gets added to the queue and 
        // when a thread becomes available, it is processed
        // This ensures that your server doesn't crash under the amount of requests

        // The thread pool automatically manages the amount of threads it needs to keep around
        // When it's created it starts out empty. As a request comes in, it creates additional 
        // threads to handle those requests

        // Find more at: https://docs.microsoft.com/en-us/dotnet/api/system.threading.threadpool?view=netframework-4.8

        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem((s)=>{
                System.Console.WriteLine("Working on a thread from threadpool");
            });
            Console.ReadLine();
        }
    }
}
