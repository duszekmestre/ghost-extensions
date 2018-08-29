using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Formatting = System.Xml.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ghost.Extensions.Attributes;
using Ghost.Extensions.Extensions.Clone;
using System.Linq.Expressions;

namespace Ghost.Extensions.Extensions
{
    public static class ObjectExtensions
    {
        public static T? Nullable<T>(this T input)
            where T : struct
        {
            return input;
        }

        public static T ValueOrDefault<T>(this T? input, T defaultValue = default(T))
            where T : struct
        {
            return input.GetValueOrDefault(defaultValue);
        }

        public static bool Deserialize<T>(string xml, out T instance)
            where T : class, new()
        {
            return GetObjectFromXml<T>(xml, out instance);
        }

        public static string SerializeToXml(this object obj, bool? indent = null)
        {
            return GenerateXml(obj, indent);
        }

        public static string SerializeWithDataContract(this object obj)
        {
            return GenerateDataContractXml(obj);
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

        public static TInput Run<TInput>(this TInput o, Action<TInput> evaluator)
            where TInput : class
        {
            if (o != null && evaluator != null)
            {
                evaluator(o);
            }

            return o;
        }

        public static void IfNotNull<TInput>(this TInput o, Action<TInput> evaluator, Action defaultAction)
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

        public static TResult TryOrDefault<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult defaultResult = default(TResult))
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
            catch (Exception exception)
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

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        //TODO: unitil we doens't use C# 6.0 and nameof
        public static string GetPropertyName<TObject, TProperty>(this TObject source, Expression<Func<TObject, TProperty>> propertyLambda)
             where TObject : class
        {
            var me = propertyLambda.Body as MemberExpression;

            if (me == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }

            return me.Member.Name;
        }

        public static object GetPropertyValue(this object input, string propertyName)
        {
            return input.GetType().GetProperty(propertyName).GetValue(input);
        }

        public static T SetIfNotNull<T>(this T item, T newObject) where T : class
        {
            var newStringObject = newObject as string;
            bool isNullOrEmpty = newStringObject != null ? string.IsNullOrWhiteSpace(newStringObject) : (newObject == null);

            if (isNullOrEmpty) return item;
            return newObject;
        }

        public static T SetIfNotDefault<T>(this T item, T newValue) where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(newValue, default(T))) return item;
            return newValue;
        }

        #region Private Methods

        private static bool GetObjectFromXml<T>(string xml, out T instance)
            where T : class, new()
        {
            instance = null;

            var serializer = new XmlSerializer(typeof(T));
            var element = XElement.Parse(xml);
            if (null == element || null == element.FirstNode)
            {
                instance = new T();
                return false;
            }

            using (var stringReader = new StringReader(element.ToString()))
            using (var xmlTextReader = new XmlTextReader(stringReader))
            {
                instance = (T)serializer.Deserialize(xmlTextReader);
            }

            return true;
        }

        private static string GenerateXml(object obj, bool? indent = null)
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            var serializer = new XmlSerializer(obj.GetType());

            var settings = new XmlWriterSettings { OmitXmlDeclaration = true };
            if (indent.HasValue)
            {
                settings.Indent = indent.Value;
            }

            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                serializer.Serialize(xmlWriter, obj, namespaces);
                return stringWriter.ToString();
            }
        }

        private static string GenerateDataContractXml(object obj)
        {
            var serializer = new DataContractSerializer(obj.GetType());
            using (var stringWriter = new StringWriter())
            using (var xmlWriter = new XmlTextWriter(stringWriter))
            {
                xmlWriter.Formatting = Formatting.None;
                serializer.WriteObject(xmlWriter, obj);
                xmlWriter.Flush();
                return stringWriter.ToString();
            }
        }

        #endregion Private Methods
    }
}
