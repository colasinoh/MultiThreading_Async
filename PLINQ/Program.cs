using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PLINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = Enumerable.Range(1, 100);

            //enforcing parallelism of LINQ query
            var fivers = numbers.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).Where( i => i % 5 == 0).ToArray();
            
            //limiting the degree of parellelism of LINQ query
            //buffer and sort using AsOrdered
            var sixers = numbers.AsParallel().AsOrdered().WithDegreeOfParallelism(5).Where(i => i % 6 == 0);

            foreach (var item in fivers)
            {
                Console.WriteLine(item);
            }

            //using ForAll to iterate over IEnumerable parallelly
            sixers.ForAll(item => Console.WriteLine("From Sixers: {0}", item));

            try
            {
                var teners = numbers.AsParallel().Where(i => IsDivisibleByTen(i));

                //teners.ForAll(item => Console.WriteLine(item));
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Number of Exceptions: {0}", e.InnerExceptions.Count);

                for (int i = 0; i < e.InnerExceptions.Count; i++)
                {
                    Console.WriteLine("Inner Exceptions: {0}", e.InnerException.Message);
                }
            }

            Console.ReadKey();
        }

        public static bool IsDivisibleByTen(int num)
        {
            if (num % 10 == 0)
            {
                throw new ArgumentException("I hit a divisible by 10!");
            }

            return num % 10 == 0;
        }
    }
}
