using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallelism
{
    class Program
    {
        static void Main(string[] args)
        {
            //prints hey 10x
            Parallel.For(0, 10, i => {
                Console.WriteLine("hey");
            });

            //prints numbers 0 - 10
            var numbers = Enumerable.Range(0, 10);

            Parallel.ForEach(numbers, i =>{
                Console.WriteLine(i);
            });

            //using ParallelLoopState object to cancel loop
            ParallelLoopResult result = Parallel.For(0, 10, (int i, ParallelLoopState loopState) => {

                if (i == 5)
                {
                    Console.WriteLine("Breaking loop...");
                    loopState.Stop();
                }

                if(!loopState.IsStopped)
                    Console.WriteLine("Counting... {0}", i);

                return;
            });
            /*note: 
             * on break, IsCompleted is false
             * on stop, LowestBreakIteration is null
            */

            Console.ReadKey();
        }
    }
}
