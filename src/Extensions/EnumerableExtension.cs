using System;
using System.Collections;
using System.Collections.Generic;

namespace covidSim.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> colletions, Action<T> act)
        {
            foreach (var elem in colletions)
            {
                act(elem);
                yield return elem;
            }
        }
    }
}