using System;
using System.Collections.Generic;

namespace Ghost.Extensions.Extensions
{
    public static class DateTimeUtilities
    {
        private static readonly DateTime TimestampDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #region Public Methods and Operators

        public static DateTime RoundDateToMinutes(this DateTime date)
        {
            var roundDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);

            roundDate = DateTime.SpecifyKind(roundDate, DateTimeKind.Utc);

            return roundDate;
        }

        public static DateTime? RoundDateToMinutes(this DateTime? date)
        {
            if (date.HasValue)
            {
                return RoundDateToMinutes(date.Value);
            }
            return null;
        }

        public static DateTimeOffset RoundDateToMinutes(this DateTimeOffset date)
        {
            var roundDate = new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0, date.Offset);
            return roundDate;
        }

        public static DateTimeOffset? RoundDateToMinutes(this DateTimeOffset? date)
        {
            return date.HasValue ? RoundDateToMinutes(date.Value).Nullable() : null;
        }

        public static long Timestamp(this DateTime date)
        {
            return (long)date.ToUniversalTime().Subtract(TimestampDateTime).TotalSeconds;
        }

        /// <summary>
        /// Will return unix like representation of the date without performing UTC conversion.
        /// This is usefull especially for provider returned dates - we cannot perfom UTC conversion on them because we have no idea of the time zone they are related with.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long DirectTimestamp(this DateTime date)
        {
            DateTime tempDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);
            return (long)tempDate.Subtract(TimestampDateTime).TotalSeconds;
        }

        public static DateTime ToDate(this long timestamp)
        {
            try
            {
                return TimestampDateTime.AddSeconds(timestamp).ToLocalTime();
            }
            catch (Exception)
            {
                return TimestampDateTime.ToLocalTime();
            }
        }

        public static long Timestamp(this DateTimeOffset date)
        {
            var timestampDateTimeOffset = new DateTimeOffset(1970, 1, 1, 0, 0, 0, date.Offset);
            return (long)date.Subtract(timestampDateTimeOffset).TotalSeconds;
        }

        public static DateTime ConvertTimeZones(this DateTime date, TimeZoneInfo source, TimeZoneInfo target)
        {
            return TimeZoneInfo.ConvertTime(date, source, target);
        }

        public static DateTime? ConvertTimeZones(this DateTime? date, TimeZoneInfo source, TimeZoneInfo target)
        {
            return date.HasValue ? TimeZoneInfo.ConvertTime(date.Value, source, target).Nullable() : null;
        }

        public static DateTimeOffset ToAnotherTimeZone(this DateTimeOffset date, TimeZoneInfo target)
        {
            return date.ToOffset(target.BaseUtcOffset);
        }

        public static DateTimeOffset? ConvertTimeZones(this DateTimeOffset? date, TimeZoneInfo target)
        {
            return date.HasValue ? date.Value.ToAnotherTimeZone(target).Nullable() : null;
        }

        public static bool Between(this DateTime date, DateTime minValue, DateTime maxValue)
        {
            return date >= minValue && date <= maxValue;
        }

        public static DateTime AddWeeks(this DateTime date, int value)
        {
            return date.AddDays(value * 7);
        }

        public static DateTime ToUtcNet(this DateTime date, string timeZoneId)
        {
            if (timeZoneId.IsNullOrWhiteSpace())
            {
                return date;
            }

            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(date, DateTimeKind.Unspecified), tz);
            }
            catch (Exception)
            {
                return date;
            }
        }

        public static DateTimeOffset ToOffset(this DateTime date, double altOffset)
        {
            return new DateTimeOffset(DateTime.SpecifyKind(date, DateTimeKind.Unspecified), TimeSpan.FromHours(altOffset));
        }

        public static bool IsPast(this DateTime date)
        {
            return date < DateTime.Now;
        }

        public static bool IsUtcPast(this DateTime date)
        {
            return date < DateTime.UtcNow;
        }

        public static string ConvertMomentJsDateFormatToNetFormat(this string momentJsFormat)
        {
            Dictionary<string, string> replacements = new Dictionary<string, string>() {
                { "Y", "y"},
                { "D", "d"},
                { "[", "'"},
                { "]", "'"},
                { "a", "tt"},
                { "/", "'/'"},
            };

            if (momentJsFormat != null)
            {
                foreach (var replacement in replacements)
                {
                    momentJsFormat = momentJsFormat.Replace(replacement.Key, replacement.Value);
                }
            }

            return momentJsFormat;
        }
        #endregion Public Methods and Operators
    }
}
