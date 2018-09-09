using Ghost.Extensions.Extensions;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Ghost.Extensions.Helpers
{
    public static class SerializationExtensions
    {
        public static void HandleDeserializationError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
        }

        private static JsonSerializerSettings CreateSerializerSettings(bool ignoreSerializationErrors)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects
            };

            if (ignoreSerializationErrors)
            {
                jsonSerializerSettings.Error += HandleDeserializationError;
            }
            
            return jsonSerializerSettings;
        }

        public static T FromJson<T>(this string json, bool ignoreSerializationErrors = false)
        {
            return json.IsNotNullOrWhiteSpace() ? JsonConvert.DeserializeObject<T>(json, CreateSerializerSettings(ignoreSerializationErrors)) : default(T);
        }

        public static object FromJson(this string json, Type typeOfValue, bool ignoreSerializationErrors = false)
        {
            return json.IsNotNullOrWhiteSpace() ? JsonConvert.DeserializeObject(json, typeOfValue, CreateSerializerSettings(ignoreSerializationErrors)) : null;
        }

        public static string ToJson<T>(this T obj, bool applySerializationSettings = false)
        {
            if (obj == null)
            {
                return null;
            }

            return applySerializationSettings
                ? JsonConvert.SerializeObject(obj, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects })
                : JsonConvert.SerializeObject(obj);
        }

        public static string ToXml<T>(this T data, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.Default;

            var inputString = new StringBuilder();
            var inputSerializer = new XmlSerializer(typeof(T));

            var xmlWriterSettings = new XmlWriterSettings
            {
                Encoding = encoding
            };

            using (var inputStream = XmlWriter.Create(inputString, xmlWriterSettings))
            {
                inputSerializer.Serialize(inputStream, data);
            }

            return inputString.ToString();
        }

        public static T FromXml<T>(this string data) where T : class
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using (var textReader = new StringReader(data))
                {
                    return xmlSerializer.Deserialize(textReader) as T;
                }
            }
            catch
            {
                return default(T);
            }
        }
    }
}
