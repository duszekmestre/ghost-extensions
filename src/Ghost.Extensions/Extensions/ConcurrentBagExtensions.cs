using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Ghost.Extensions.Extensions
{
    public static class ConcurrentBagExtensions
    {
        public static void RemoveItem<T>(this ConcurrentBag<T> bag, T item)
        {
            bag = new ConcurrentBag<T>(bag.Except(new[] { item }));
        }

        public static void RemoveAll<T>(this ConcurrentBag<T> bag, Func<T, bool> func)
        {
            bag = new ConcurrentBag<T>(bag.Except(bag.Where(func)));
        }

        public static ConcurrentBag<T> Without<T>(this ConcurrentBag<T> bag, T item)
        {
            return new ConcurrentBag<T>(bag.Except(new[] { item }));
        }

        public static ConcurrentBag<T> Without<T>(this ConcurrentBag<T> bag, Func<T, bool> func)
        {
            return new ConcurrentBag<T>(bag.Except(bag.Where(func)));
        }
    }
}
