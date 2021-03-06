using System;
using System.Collections.Generic;

namespace Linq.Sample02
{
    public static class MyLinq
    {
        //sample code for deffered execution
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source,
                                               Func<T,bool> predicate)
        {
            
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }
    }
}
