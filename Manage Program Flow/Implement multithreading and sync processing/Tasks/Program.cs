using System;
using System.Threading.Tasks;

namespace Tasks
{
    class Program
    {
        // Simple example of Task
        public static void SimpleTask()
        {
            // Create a task and immediately start it
            // Wait is equal to Thread.Join, waits till the Task is finished before exiting the app
            Task t = Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine('*');
                }
            });
            t.Wait();
        }

        // Simple example of Task<T>
        // Task<T> should be used if Task should return a value
        public static void TaskT()
        {
            Task<int> t = Task.Run(() =>
            {
                return 42;
            }).ContinueWith((i) => 
            // It's possible to add continuation Task, it will execute another Task
            // after the original task is finished
            // i=42 here, it's a result of previous task
            {
                return i.Result * 2;
            });

            // Attempting to read the Result property will force Thread
            // that's trying to read the result to wait until the Task
            // is finished before continuing.
            // As long as the Task not finished impossible to get the Result
            // If the Task is not finished it will block the current thread
            Console.WriteLine(t.Result);
        }

        // ContinueWith has a couple of overloads
        // It allows to add different continuation methods
        // that will run when an exception happens,
        // the Task is canceled, or succeeded
        public static void ContinueWith()
        {
            Task<int> t = Task.Run(()=> 
            {
                return 42;
            });

            // Cancel
            t.ContinueWith((i) =>
            {
                Console.WriteLine("Canceled");
            }, TaskContinuationOptions.OnlyOnCanceled);

            // Faulted
            t.ContinueWith((i) =>
            {
                Console.WriteLine("Faulted");
            }, TaskContinuationOptions.OnlyOnFaulted);

            // Completed
            var completedTask = t.ContinueWith((i) =>
            {
                Console.WriteLine("Completed");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            // Wait for execution
            completedTask.Wait();
        }

        // Child tasks
        // Parent task finished when ALL child tasks are ready
        public static void ChildTasks()
        {
            Task<int[]> parent = Task.Run(() =>
            {
                var results = new int[3];
                new Task(() => results[0] = 0, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = 1, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = 2, TaskCreationOptions.AttachedToParent).Start();
                return results;
            });

            var finalTask = parent.ContinueWith(
                parentTask =>
                {
                    foreach (int i in parentTask.Result)
                    {
                        Console.WriteLine(i);
                    }
                });
            // The finalTask runs only after the parent Task is finished
            // and the parent Task finishes when all 3 children are finished.
            // Can be used to create complex Task hierarchies that will
            // go through all the steps you specified.
            finalTask.Wait();
        }

        // TaskFactory
        // No need to specify TaskCreationOptions.AttachedToParent
        // to each task
        // A TaskFactory is created with a certain configu-ration and 
        // can then be used to create Tasks with that configuration.
        public static void TaskFactory()
        {
            Task<int[]> parent = Task.Run(() =>
            {
                var results = new int[3];
                TaskFactory tf = new TaskFactory(TaskCreationOptions.AttachedToParent,
                    TaskContinuationOptions.ExecuteSynchronously);
                tf.StartNew(() => results[0] = 0);
                tf.StartNew(() => results[1] = 1);
                tf.StartNew(() => results[2] = 2);
                return results;
            });

            var finalTask = parent.ContinueWith(
                parentTask =>
                {
                    foreach (int i in parent.Result)
                        Console.WriteLine(i);
                });

            finalTask.Wait();
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"SimpleTask:{Environment.NewLine}");
            SimpleTask();
            Console.ReadKey();

            Console.WriteLine($"TaskT:{Environment.NewLine}");
            TaskT();
            Console.ReadKey();

            Console.WriteLine($"ContinueWith:{Environment.NewLine}");
            ContinueWith();
            Console.ReadKey();

            Console.WriteLine($"ChildTasks:{Environment.NewLine}");
            ChildTasks();
            Console.ReadKey();

            Console.WriteLine($"TaskFactory:{Environment.NewLine}");
            TaskFactory();
            Console.ReadKey();
        }
    }
}
