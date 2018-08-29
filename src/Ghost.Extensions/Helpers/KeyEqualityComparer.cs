using System;
using System.Collections.Generic;

namespace Ghost.Extensions.Helpers
{
    public class KeyEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, object> keyExtractor;

        public KeyEqualityComparer(Func<T, object> keyExtractor)
        {
            this.keyExtractor = keyExtractor;
        }

        public bool Equals(T x, T y)
        {
            return this.keyExtractor(x).Equals(this.keyExtractor(y));
        }

        public int GetHashCode(T obj)
        {
            return this.keyExtractor(obj).GetHashCode();
        }
    }

    public static class EqualityExtension
    {
        public static KeyEqualityComparer<T> EqualBy<T>(this T obj, Func<T, object> keyExtractor)
        {
            return new KeyEqualityComparer<T>(keyExtractor);
        }
    }
}
