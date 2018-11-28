using System;
using System.Collections.Generic;

namespace FibonacciCaching.Cache
{
    public interface IFibonacciCache
    {
        List<ulong> Get(string step);
        void Set(string step, List<ulong> result);
    }
}