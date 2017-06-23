using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace ConcurrentCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            Task[] tasks = new Task[4];

            //using BlockingCollection - thread-safe for adding & removing data
            BlockingCollection<string> bCol = new BlockingCollection<string>();
            
            tasks[0] = Task.Run(() => {
                
                foreach (var bColItem in bCol.GetConsumingEnumerable())
                    Console.WriteLine(bColItem);

            });

            tasks[1] = Task.Run(() => {

                while (true)
                {
                    string input = Console.ReadLine();

                    if (input.Equals("hey"))
                    {
                        bCol.CompleteAdding();
                        break;
                    }

                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        bCol.Add(input);
                    }
                    else
                        break;
                }

            });

            //write.Wait();

            Console.WriteLine("Continued to ConcurrentBag...");
            
            //using ConcurrentBag
            ConcurrentBag<int> cBag = new ConcurrentBag<int>();

            for (int i = 0; i < 10; i++)
            {
                cBag.Add(i);
            }

            int item;

            if (cBag.TryTake(out item))
                Console.WriteLine(item);

            if (cBag.TryPeek(out item))
                Console.WriteLine("There is a next item: {0}", item);

            /**ConcurrentBag implements IEnumerable<T> by making a snapshot of the collection
             * when you start iterating over it
            **/

            ConcurrentBag<string> seasons = new ConcurrentBag<string>();

            tasks[2] = Task.Run(() => {

                seasons.Add("Summer");
                seasons.Add("Winter");
                seasons.Add("a");
                seasons.Add("b");
                seasons.Add("c");
                Thread.Sleep(5000);
                seasons.Add("d");
                seasons.Add("e");
                seasons.Add("f");
                seasons.Add("g");

            });

            tasks[3] = Task.Run(() =>
            {
                foreach (string season in seasons)
                    Console.WriteLine(season);
            });

            Task.WaitAll(tasks);

            Console.WriteLine("Contined to ConcurrentStack...");
            
            //using ConcurrentStack
            ConcurrentStack<int> cStack = new ConcurrentStack<int>();

            cStack.Push(100);

            int stackItem;

            if (cStack.TryPop(out stackItem))
                Console.WriteLine("Popped: {0}", stackItem);

            cStack.PushRange(new int[] { 101, 102, 103 });

            int[] values = new int[2];
            cStack.TryPopRange(values);

            foreach (int num in cStack)
                Console.WriteLine("Remaining item/s: {0}", num); //101

            Console.WriteLine("Continued to ConcurrentQueue...");
            
            //using ConcurrentQueue
            ConcurrentQueue<string> cQueue = new ConcurrentQueue<string>();

            cQueue.Enqueue("person1");
            cQueue.Enqueue("person2");
            cQueue.Enqueue("person3");

            string removeOrNext;

            if (cQueue.TryDequeue(out removeOrNext))
                Console.WriteLine("Person removed: {0}", removeOrNext);

            if (cQueue.TryPeek(out removeOrNext))
                Console.WriteLine("Next person is: {0}", removeOrNext);

            Console.WriteLine("Remaining Persons is/are:");
            foreach (string person in cQueue)
                Console.WriteLine(person);

            Console.WriteLine("Continued to ConcurrentDictionary...");

            //using ConcurrentDictionary
            ConcurrentDictionary<int, string> cDictionary = new ConcurrentDictionary<int, string>();

            cDictionary.TryAdd(1, "apple"); //apple is my favorite
            cDictionary.TryUpdate(1, "watermelon", "apple"); //nope, mango is my favorite
            cDictionary[1] = "mango"; //yes, mango is my favorite
            cDictionary.AddOrUpdate(1, "orange", (a, b) => b + "es"); //let's make it plural
            cDictionary.GetOrAdd(1, "santol");

            foreach (var fruit in cDictionary)
                Console.WriteLine(fruit); //here's my favorite fruit!

            Console.ReadKey();
        }
    }
}
