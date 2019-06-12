using System;
using System.Threading;

namespace mThread
{
    class Program
    {
        // Foreground threads can be used to keep app alive
        // Background threads are terminated when app is shut down 
        // (when all foreground threads end)

        public static void BackgroundThread()
        {
            Thread t = new Thread(() =>
            {
                Console.WriteLine("Some thread action");
            });
            // Specify background thread
            t.IsBackground = true;
            t.Start();
        }

        // Parametrized thread

        public static void ThreadMethod(object o)
        {
            for (int i = 0; i < (int)o; i++)
            {
                Console.WriteLine("Thread Proc: {0}", i);
                // Signal to Windows that this thread is finished 
                Thread.Sleep(0);
            }
        }

        public static void ParameterizedThread()
        {
            Thread t = new Thread(new ParameterizedThreadStart(ThreadMethod));
            // Passing an argument to method ThreadMethod
            t.Start(5);
            // Blocks the calling thread until the thread represented by this instance terminates.
            t.Join();
        }

        // Stopping a thread
        public static void ThreadStop()
        {
            bool stopped = false;

            Thread t = new Thread(new ThreadStart(() =>
            {
                while (!stopped)
                {
                    Console.WriteLine("Running...");
                    Thread.Sleep(1000);
                }
            }));

            t.Start();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            stopped = true;
            t.Join();
        }

        // Using a ThreadStatic attribute
        // Indicates that the value of a static field is unique for each thread.
        [ThreadStatic]
        public static int _field;

        public static void ThreadStaticAttribute()
        {
            Thread t1 = new Thread(() => {
                for (int i = 0; i < 10; i++)
                {
                    System.Console.WriteLine("Thread A: {0}", ++_field);
                }
            });

            Thread t2 = new Thread(() => {
                for (int i = 0; i < 10; i++)
                {
                    System.Console.WriteLine("Thread B: {0}", ++_field);
                }
            });
            t1.Start();
            t1.Join();
            t2.Start();
            t2.Join();
        }

        //  Provides thread-local storage of data.
        public static ThreadLocal<int> _fieldLocal = new ThreadLocal<int>(() => {
            return Thread.CurrentThread.ManagedThreadId;
        });

        public static void ThreadLocalData()
        {
            new Thread(() => {
                for (int i = 0; i < _fieldLocal.Value; i++)
                {
                    System.Console.WriteLine("Thread A: {0}", i);
                }
            }).Start();
            new Thread(() => {
                for (int i = 0; i < _fieldLocal.Value; i++)
                {
                    System.Console.WriteLine("Thread B: {0}", i);
                }
            }).Start();
        }

        public static void Main(){

            Console.WriteLine($"BackgroundThread:{Environment.NewLine}");
            BackgroundThread();
            Console.ReadKey();

            Console.WriteLine($"ParameterizedThread Method:{Environment.NewLine}");
            ParameterizedThread();
            Console.ReadKey();

            Console.WriteLine($"ThreadStop Method:{Environment.NewLine}");
            ThreadStop();
            Console.ReadKey();


            Console.WriteLine($"ThreadStaticAttribute Method:{Environment.NewLine}");
            ThreadStaticAttribute();
            Console.ReadKey();

            Console.WriteLine($"ThreadLocalData Method:{Environment.NewLine}");
            ThreadLocalData();
            Console.ReadKey();

            System.Diagnostics.Debugger.Break();
            
#if DEBUG
            Console.WriteLine("Debug");
#endif
            Console.ReadKey();
        }
    }
}
