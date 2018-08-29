namespace Ghost.Extensions.Helpers
{
    public static class PropertyHelper
    {
        public static object GetPropertyValue<T>(this T src, string propPath)
        {
            if (propPath.Contains("."))//complex type nested
            {
                var temp = propPath.Split(new char[] { '.' }, 2);
                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
            }
            
            var prop = typeof(T).GetProperty(propPath);
            return prop?.GetValue(src, null);
        }
    }
}
