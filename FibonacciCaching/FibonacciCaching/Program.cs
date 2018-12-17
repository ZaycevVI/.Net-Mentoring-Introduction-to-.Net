using System;
using System.Collections.Generic;
using System.Diagnostics;
using FibonacciCaching.Cache;
using FibonacciCaching.Service;

namespace FibonacciCaching
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceWithCache = new FibonacciService(new FibonacciMemoryCache(), true);
            var serviceWithoutCache = new FibonacciService(new FibonacciMemoryCache(), false);

            do
            {
                Console.Clear();
                Console.WriteLine("Введите номер позиции для вычисления последовательности Фибоначчи: ");
                var isParsed = uint.TryParse(Console.ReadLine(), out var step);

                if(!isParsed)
                    break;
                List<ulong> result = null;
                var ticksWithCache = CalculateTime(() => result = serviceWithCache.Calculate(step));
                var ticksWithoutCache = CalculateTime(() => serviceWithoutCache.Calculate(step));
                Display(result);
                Console.WriteLine("Calculation time with cache: {0} elapsed ticks.", ticksWithCache);
                Console.WriteLine("Calculation time without cache: {0} elapsed ticks.", ticksWithoutCache);
                Console.ReadLine();
            } while (true);
        }

        private static void Display(IEnumerable<ulong> args)
        {
            Console.WriteLine("======================");
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }
            Console.WriteLine("======================");

        }

        private static long CalculateTime(Action func)
        {
            var watch = Stopwatch.StartNew();
            func();
            watch.Stop();

            return watch.ElapsedTicks;
        }
    }
}
