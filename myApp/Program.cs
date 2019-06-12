using System;
using System.Threading;

namespace mThread
{
    class Program
    {
        [ThreadStatic]
        public static int _field;

        public static ThreadLocal<int> _fieldLocal = new ThreadLocal<int>(()=>{
            return Thread.CurrentThread.ManagedThreadId;
        });

        public static void Main(){
            Thread t1 = new Thread(()=>{
                for(int i=0;i<10;i++){
                    System.Console.WriteLine("Thread A: {0}", ++_field);
                }
            });

            Thread t2 = new Thread(()=>{
                for(int i=0;i<10;i++){
                    System.Console.WriteLine("Thread B: {0}", ++_field);
                }
            });
            t1.Start();
            t1.Join();
            t2.Start();
            t2.Join();
            Console.ReadKey();
            System.Console.WriteLine();
            new Thread(()=>{
                for (int i = 0; i < _fieldLocal.Value; i++)
                {
                    System.Console.WriteLine("Thread A: {0}", i);
                }
            }).Start();
            new Thread(()=>{
                for (int i = 0; i < _fieldLocal.Value; i++)
                {
                    System.Console.WriteLine("Thread B: {0}", i);
                }
            }).Start();
            Console.ReadKey();
            System.Diagnostics.Debugger.Break();
            
#if DEBUG
            Console.WriteLine("Debug");
#endif
            Console.ReadKey();
        }
    }
}
