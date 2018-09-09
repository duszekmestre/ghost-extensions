using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ghost.Extensions.Extensions
{
    public static class StringExtensions
    {
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }        

        public static string SkipString(this string input, int skip)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            return new string(input.Skip(skip).ToArray());
        }

        public static string TakeString(this string input, int take)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            return new string(input.Take(take).ToArray());
        }

        public static string RemoveString(this string input, string phrase)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            return input.Replace(phrase, string.Empty);
        }

        public static string RemoveMultipleWhitespaces(this string input)
        {
            var spaceArray = new[] {' '};
            return string.Join(" ", input.Split(spaceArray, StringSplitOptions.RemoveEmptyEntries)).Trim();
        }

        public static int AsInt(this string input, int defaultValue = 0)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return defaultValue;
            }

            return int.TryParse(input, out int output) ? output : defaultValue;
        }

        public static bool AsBool(this string input, bool defaultValue = false)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return defaultValue;
            }

            return bool.TryParse(input, out bool output) ? output : defaultValue;
        }

        public static DateTime AsDateTime(this string input, string format)
        {
            return DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) ? date : default(DateTime);
        }

        public static DateTime? AsDateTimeNullable(this string input, string format)
        {
            return DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) ? date.AsNullable() : null;
        }

        public static TimeSpan AsTimeSpan(this string input, string format = "c")
        {
            return TimeSpan.TryParseExact(input, format, CultureInfo.InvariantCulture, out TimeSpan timespan) ? timespan : default(TimeSpan);
        }

        public static decimal AsDecimal(this string input, decimal defaultValue = decimal.Zero)
        {
            return decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal output) ? output : defaultValue;
        }

        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static bool IsNotNullOrWhiteSpace(this string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        public static bool IsNotNullOrEmpty(this string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        public static string Overflow(this string input, int maxLength, string postfix = null)
        {
            if (!string.IsNullOrWhiteSpace(postfix))
            {
                maxLength -= postfix.Length;
            }

            return input.TakeString(maxLength) + (postfix ?? string.Empty);
        }

        public static byte[] GetBytes<T>(this string input)
            where T : Encoding, new()
        {
            var encoding = new T();
            return encoding.GetBytes(input);
        }

        public static string HtmlEncodeCurrencies(this string text)
        {
            var sb = new StringBuilder(text);

            return sb.Replace("€", "&#8364;")
                     .Replace("$", "&#36;")
                     .Replace("¥", "&#165;")
                     .Replace("£", "&#163;").ToString();
        }

        public static string Surround(this string parameter, string left, string right)
        {
            return string.Format("{0}{1}{2}", left, parameter, right);
        }

        public static T ToEnum<T>(this string input, T defaultValue = default(T))
            where T : struct
        {
            if (Enum.TryParse(input, out T enumValue))
            {
                return enumValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static string Compress(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            try
            {
                byte[] encoded = Encoding.UTF8.GetBytes(input);
                byte[] compressed = Compress(encoded);
                return Convert.ToBase64String(compressed);
            }
            catch (Exception)
            {
                return input;
            }
        }

        private static byte[] Compress(byte[] input)
        {
            using (var result = new MemoryStream())
            {
                var lengthBytes = BitConverter.GetBytes(input.Length);
                result.Write(lengthBytes, 0, 4);

                using (var compressionStream = new GZipStream(result, CompressionMode.Compress))
                {
                    compressionStream.Write(input, 0, input.Length);
                    compressionStream.Flush();

                }
                return result.ToArray();
            }
        }

        public static string Decompress(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            try
            {
                byte[] compressed = Convert.FromBase64String(input);
                byte[] decompressed = Decompress(compressed);
                return Encoding.UTF8.GetString(decompressed);
            }
            catch
            {
                return input;
            }
        }

        private static byte[] Decompress(byte[] input)
        {
            using (var source = new MemoryStream(input))
            {
                byte[] lengthBytes = new byte[4];
                source.Read(lengthBytes, 0, 4);

                var length = BitConverter.ToInt32(lengthBytes, 0);
                using (var decompressionStream = new GZipStream(source, CompressionMode.Decompress))
                {
                    var result = new byte[length];
                    decompressionStream.Read(result, 0, length);
                    return result;
                }
            }
        }

        public static bool IsBase64String(this string input)
        {
            return input.IsNotNullOrWhiteSpace() && (input.Trim().Length % 4 == 0) && Regex.IsMatch(input.Trim(), @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        public static string RemoveAllWhiteSpaces(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                StringBuilder sb = new StringBuilder(input.Length);
                for (int i = 0; i < input.Length; i++)
                {
                    char c = input[i];
                    if (!char.IsWhiteSpace(c))
                    {
                        sb.Append(c);
                    }
                }

                return sb.ToString();
            }
            else
            {
                return input;
            }
        }

        public static IEnumerable<string> ToLowerInvariant(this IEnumerable<string> enumerable)
        {
            var lowerCaseEnumerable = new List<string>();
            enumerable.ForEach(x => lowerCaseEnumerable.Add(x.ToLowerInvariant()));
            return lowerCaseEnumerable;
        }

        public static string CleanFileName(this string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

    }
}