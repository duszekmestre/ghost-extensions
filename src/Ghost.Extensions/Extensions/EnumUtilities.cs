namespace Ghost.Extensions.Extensions
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.Serialization;
    using System.Threading;
    using Attributes;

    public static class EnumUtilities
    {
        public struct EnumTranslation
        {
            public Enum Enum { get; set; }

            public string LanguageCode { get; set; }

            public EnumTranslation(Enum e, string langCode)
            {
                this.Enum = e;
                this.LanguageCode = langCode;
            }

            public override string ToString()
            {
                return $"{this.Enum}/{this.LanguageCode}";
            }
        }

        #region Public Methods and Operators

        public static string ToUserString(this Enum enumeration)
        {
            var type = enumeration.GetType();
            var field = type.GetField(enumeration.ToString());

            if (field == null)
            {
                return string.Empty;
            }

            var enumString = field.GetCustomAttribute<EnumStringAttribute>(true);
            return enumString != null ? enumString.ToString() : enumeration.ToString();
        }

        private static ConcurrentDictionary<string, ConcurrentDictionary<Enum, string>> dict = new ConcurrentDictionary<string, ConcurrentDictionary<Enum, string>>();

        public static string ToResourceString(this Enum enumeration, string langCode = "")
        {
            if (string.IsNullOrEmpty(langCode))
            {
                langCode = Thread.CurrentThread.CurrentCulture.Name; // "en-GB"
            }

            var langDict = GetLanguageDict(langCode);

            return langDict.GetOrAdd(enumeration, (key) =>
            {
                var enumType = key.GetType();

                var enumResurce = enumType.GetCustomAttribute<EnumResourceAttribute>();
                if (enumResurce == null)
                {
                    return enumeration.ToString();
                }

                var trans = GetEnumTranslationFromResources(enumeration, langCode);
                return trans;
            });
        }

        private static ConcurrentDictionary<Enum, string> GetLanguageDict(string langCode)
        {
            return dict.GetOrAdd(langCode, new ConcurrentDictionary<Enum, string>());
        }

        private static readonly ConcurrentDictionary<string, ResourceManager> Managers = new ConcurrentDictionary<string, ResourceManager>();

        private static ResourceManager GetResourceManager(string fullName, Assembly assembly)
        {
            return Managers.GetOrAdd(fullName, resourceType => new ResourceManager(fullName, assembly));
        }

        /// <exception cref="ReflectionTypeLoadException">The assembly contains one or more types that cannot be loaded. The array returned by the <see cref="P:System.Reflection.ReflectionTypeLoadException.Types" /> property of this exception contains a <see cref="T:System.Type" /> object for each type that was loaded and null for each type that could not be loaded, while the <see cref="P:System.Reflection.ReflectionTypeLoadException.LoaderExceptions" /> property contains an exception for each type that could not be loaded.</exception>
        /// <exception cref="AmbiguousMatchException">More than one of the requested attributes was found. </exception>
        /// <exception cref="TypeLoadException">A custom attribute type cannot be loaded. </exception>
        public static void InitGetResourceManager(Assembly assembly)
        {
            var enumTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsEnum && t.IsPublic);

            foreach (var enumResurce in enumTypes.Select(enumType => enumType.GetCustomAttribute<EnumResourceAttribute>()).Where(enumResurce => enumResurce != null))
            {
                GetResourceManager(enumResurce.ResourceType.FullName, assembly);
            }
        }

        /// <summary>
        /// Gets the enum translation from resources.
        /// </summary>
        /// <param name="enumeration">The enumeration.</param>
        /// <param name="langCode">The language code.</param>
        /// <returns></returns>
        public static string GetEnumTranslationFromResources(Enum enumeration, string langCode)
        {
            var enumType = enumeration.GetType();
            var enumResurce = enumType.GetCustomAttribute<EnumResourceAttribute>();
            if (enumResurce == null || enumResurce.ResourceType == null) return enumeration.ToString();
            try
            {
                if (string.IsNullOrWhiteSpace(enumResurce.Prefix))
                {
                    enumResurce.Prefix = enumType.Name + "_";
                }

                var langCulture = new CultureInfo(langCode);
                var resourceManager = GetResourceManager(enumResurce.ResourceType.FullName, enumResurce.ResourceType.Assembly);
                var resourceString = resourceManager.GetString(enumResurce.Prefix + enumeration, langCulture);
                if (!string.IsNullOrWhiteSpace(resourceString))
                {
                    return resourceString;
                }
            }
            catch (Exception)
            {
            }

            return enumeration.ToString();
        }

        public static TResult Remap<TInput, TResult>(this TInput input, Dictionary<TInput, TResult> map,
            TResult defaultVal = default(TResult))
        {
            if (map != null && map.ContainsKey(input))
            {
                return map[input];
            }

            return defaultVal;
        }        

        public static T ParseEnum<T>(this string text)
            where T : struct
        {
            T e;
            return Enum.TryParse(text, true, out e) ? e : default(T);
        }

        #endregion Public Methods and Operators
    }
}