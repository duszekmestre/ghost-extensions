using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Ghost.Extensions.Extensions.Clone;

namespace Ghost.Extensions.Extensions
{
    public static class ObjectExtensions
    {
        public static T? AsNullable<T>(this T input)
            where T : struct
        {
            return input;
        }

        public static TOut Map<TOut>(this object input)
        {
            return JsonConvert.DeserializeObject<TOut>(JsonConvert.SerializeObject(input));
        }

        public static T DeepClone<T>(this T input)
        {
            if (input == null)
            {
                return input;
            }

            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(input, settings), settings);
        }

        /// <summary>
        /// Very experimental :)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T FastDeepClone<T>(this T input)
        {
            if (input == null)
            {
                return input;
            }

            return ExpressionTreeCloner.DeepFieldClone(input);
        }

        public static TResult IfNotNull<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            return o == null ? null : evaluator(o);
        }

        public static void IfNotNullOrDefault<TInput>(this TInput o, Action<TInput> evaluator, Action defaultAction)
            where TInput : class
        {
            if (o != null && evaluator != null)
            {
                evaluator(o);
                return;
            }

            defaultAction();
        }

        public static TResult IfNotNullOrDefault<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult defaultResult = default(TResult))
            where TInput : class
        {
            return o == null ? defaultResult : evaluator(o);
        }

        public static TInput TryInvoke<TInput>(this TInput o, Action<TInput> evaluator)
            where TInput : class
        {
            if (o != null && evaluator != null)
            {
                evaluator(o);
            }

            return o;
        }

        public static TResult TryInvokeOrDefault<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult defaultResult = default(TResult))
            where TInput : class
        {
            if (evaluator == null)
            {
                return defaultResult;
            }

            try
            {
                return o.IfNotNullOrDefault(evaluator, defaultResult);
            }
            catch
            {
                return defaultResult;
            }
        }

        public static List<T> AsList<T>(this T value)
        {
            return value == null ? new List<T>() : new List<T> { value };
        }

        public static T[] AsArray<T>(this T value)
        {
            if (value == null)
            {
                return null;
            }

            return new[] { value };
        }

        public static TArray[] AsArray<T, TArray>(this T value)
            where T : TArray
        {
            if (value == null)
            {
                return null;
            }

            return new TArray[] { value };
        }
    }
}
