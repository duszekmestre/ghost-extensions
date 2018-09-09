using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ghost.Extensions.Extensions
{
    public static class EnumerableExtension
    {
        private static readonly Random Rand = new Random();

        public static TSource Random<TSource>(this IEnumerable<TSource> source)
        {
            return source.RandomOrDefault(default(TSource));
        }

        public static TSource RandomOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue =  default(TSource))
        {
            if (source == null)
            {
                return defaultValue;
            }

            var coll = source.ToList();
            if (!coll.Any())
            {
                return defaultValue;
            }

            var count = coll.Count;
            return coll[Rand.Next(0, count)];
        }

        public static string GetIndexOrEmpty(this IEnumerable<string> enumerable, int index)
        {
            return enumerable.GetIndexOrDefault(index, string.Empty);
        }

        public static T GetIndexOrDefault<T>(this IEnumerable<T> enumerable, int index, T defaultValue = default(T))
        {
            var array = enumerable as T[] ?? enumerable.ToArray();
            if (array.Length > index)
            {
                return array[index];
            }

            return defaultValue;
        }

        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source == null || !source.Any();
        }

        public static bool AnyAndAll<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return !source.IsNullOrEmpty() && source.All(predicate);
        }

        /// <summary>
        /// Method to remove duplicated rows.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in collection</typeparam>
        /// <typeparam name="TSelector">Selector func to retrieve properties from <see cref="TSource"/> type to compare</typeparam>
        /// <param name="source">Collection to find duplicates</param>
        /// <param name="selector">Selection object.</param>
        /// <param name="includeNulls">Id true then null values will be included in result collection (only one - distinct)</param>
        /// <returns>Collection with removed duplicated values.</returns>
        public static IEnumerable<TSource> Distinct<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource,TSelector> selector, bool includeNulls = false)
        {
            var sourceList = source as IList<TSource> ?? source.ToList();
            var isNull = sourceList.Any(x => x == null);
            var result = sourceList.Where(x => x != null).GroupBy(selector).Select(g => g.FirstOrDefault()).Where(x => x != null);
            
            if (includeNulls && isNull)
            {
                return result.Concat(new[] {default(TSource)});
            }

            return result;
        }

        public static bool AllSame<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector)
        {
            return source.Distinct(selector).AllSame();
        }

        public static bool AllSame<TSource>(this IEnumerable<TSource> source)
        {
            return source.Distinct().Count() == 1;
        }

        public static bool In<TSource>(this TSource value, IEnumerable<TSource> collection)
        {
            return collection.Contains(value);
        }

        public static bool In<TSource>(this TSource value, params TSource[] collection)
        {
            return collection != null && collection.Contains(value);
        }

        public static bool NotIn<TSource>(this TSource value, params TSource[] collection)
        {
            return !value.In(collection);
        }

        public static async Task ParallelForEach<T>(this IEnumerable<T> collection, Func<T, Task> action, int maxConcurrency = 10)
        {
            if (collection == null || action == null)
            {
                return;
            }

            try
            {
                await Task.WhenAll(Partitioner.Create(collection).GetPartitions(maxConcurrency).Select(partition => Task.Run(async delegate
                {
                    using (partition)
                    {
                        while (partition.MoveNext())
                        {
                            try
                            {
                                await action(partition.Current);
                            }
                            catch
                            {
                            }
                        }
                    }
                })));
            }
            catch
            {                
            }
        }

        public static void ParallelForEach<T>(this IEnumerable<T> collection, Action<T> action, int maxConcurrency = 10)
        {
            if (collection == null || action == null)
            {
                return;
            }

            collection
                .AsParallel()
                .WithDegreeOfParallelism(maxConcurrency)
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(x =>
                {
                    action(x);
                    return true;
                })
                .ToList();
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection.IsNullOrEmpty())
            {
                return;
            }

            foreach (var el in collection)
            {
                action.Invoke(el);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int> action)
        {
            if (collection.IsNullOrEmpty())
            {
                return;
            }

            var invocationList = collection.Select((x, i) =>
            {
                action.Invoke(x, i);
                return true;
            }).ToList();
        }

        public static bool HasAnyElement<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.Any();
        }



        public static IEnumerable<T> Concat<T>(this IEnumerable<T> collection, T item)
		{
            if (item == null)
            {
                return collection;
            }

			return collection.Concat(item.AsArray());
		}

        public static IEnumerable<T> Touch<T>(this IEnumerable<T> collection)
        {
            return collection ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<TSource> Flatten<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TSource>> getChildrenFunction)
        {
            var flattenedList = source;

            foreach (TSource element in source)
            {
                flattenedList = flattenedList.Concat(getChildrenFunction(element).Flatten(getChildrenFunction));
            }

            return flattenedList;
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static string Join<T>(this IEnumerable<T> input, string separator)
        {
            return string.Join(separator, input);
        }
    }
}
