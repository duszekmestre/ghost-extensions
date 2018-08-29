using System.Collections.Generic;

namespace Ghost.Extensions.Helpers
{
    public class ReferenceComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            return ReferenceEquals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }

    public static class ReferenceComparerExtensions
    {
        public static ReferenceComparer<T> ReferenceEqual<T>(this T obj)
        {
            return new ReferenceComparer<T>();
        }
    }
}
