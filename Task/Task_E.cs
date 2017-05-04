using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_E
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = Task.Run(()=>
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine(i);
                }
            });

            //task that returns a value
            Task<int> t2 = Task.Run(() =>
            {
                return 1;
            });

            Console.WriteLine(t2.Result);

            //using continuation
            Task<int> t3 = Task.Run(() =>
            {
                return 2;
            }).ContinueWith(i =>
            {
                return i.Result * 2;
            });

            Console.WriteLine("T3: " + t3.Result);

            //using task continuation options
            Task<int> t4 = Task.Run(() =>
            {
                return 5;
            });

            t4.ContinueWith(i =>
            {
                Console.WriteLine("Cancelled");
            }, TaskContinuationOptions.OnlyOnCanceled).ContinueWith(i =>{
                Console.Write("Faulted");
            }, TaskContinuationOptions.OnlyOnFaulted).ContinueWith((i) => {
                Console.WriteLine("Completed");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            t4.Wait();
            Console.Write("t4 status: " + t4.Status);

            //using child tasks
            Task<Int16[]> parentTask = Task.Run(() => {
                var childTasks = new Int16[3];

                new Task(() => {
                    childTasks[0] = 0;
                }, TaskCreationOptions.AttachedToParent).Start();

                new Task(() => childTasks[1] = 1, TaskCreationOptions.AttachedToParent).Start();

                new Task(() => {
                    childTasks[2] = 2;
                }, TaskCreationOptions.AttachedToParent).Start();

                return childTasks;
            });

            parentTask.ContinueWith( i =>
            {
                foreach (var item in parentTask.Result)
                {
                    Console.WriteLine("Child task: " + item);       
                }
            }).Wait();

            //using taskFactory
            Task<Int16[]> parentTask2 = Task.Run(() => {
                var factoryTasks = new Int16[3];

                TaskFactory tf = new TaskFactory(TaskCreationOptions.AttachedToParent, TaskContinuationOptions.ExecuteSynchronously);

                tf.StartNew(() => factoryTasks[0] = 0);
                tf.StartNew(() => factoryTasks[1] = 1);
                tf.StartNew(() => factoryTasks[2] = 2);

                return factoryTasks;
            });

            parentTask2.ContinueWith(i => {
                foreach (var item in parentTask2.Result)
                {
                    Console.WriteLine("Child TaskFactory: " + item);
                }
            }).Wait();

            //using WaitAll
            Task[] tasks = new Task[3];

            tasks[0] = Task.Run(() => {
                Console.WriteLine("Task 1");
                return 1;
            });

            tasks[1] = Task.Run(() =>{
                Console.WriteLine("Task 2");
                return 2;
            });

            tasks[2] = Task.Run(() => {
                Console.WriteLine("Task 3");
                return 3;
            });
            
            Task.WaitAll(tasks);

            //using Task.WaitAny

            Console.ReadKey();
        }
    }
}