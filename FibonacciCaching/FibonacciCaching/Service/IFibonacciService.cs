using System.Collections.Generic;

namespace FibonacciCaching.Service
{
    public interface IFibonacciService
    {
        List<ulong> Calculate(uint step);
    }
}