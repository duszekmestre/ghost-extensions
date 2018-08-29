using System.Linq;
using Ghost.Extensions.Helpers;

namespace Ghost.Extensions.Extensions
{
    public static class TranslatorHelper
    {
        public static T MapTo<T>(this object source)
            where T : new()
        {
            if (source == null)
            {
                return default(T);
            }

            var sourceProperties = source.GetType().GetProperties();
            var destinationProperties = typeof (T).GetProperties();

            var response = new T();

            foreach (var property in sourceProperties)
            {
                var sameProperty = destinationProperties.FirstOrDefault(x => x.Name == property.Name && x.PropertyType == property.PropertyType);
                if (sameProperty != null)
                {
                    sameProperty.SetValue(response, property.GetValue(source));
                }
            }

            return response;
        }

        public static T JsonMap<T>(this object source)
        {
            return source.ToJson().FromJson<T>();
        }
    }
}
