namespace Ghost.Extensions.Extensions
{
    public static class NumericExtensions
    {
        public static bool IsBetween(this int value, int min, int max)
        {
            return value > min && value < max;
        }

        public static bool IsBetweenOrEqual(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        public static bool ISBetween(this decimal value, decimal min, decimal max)
        {
            return value > min && value < max;
        }

        public static bool IsBetweenOrEqual(this decimal value, decimal min, decimal max)
        {
            return value >= min && value <= max;
        }

        public static bool ISBetween(this double value, double min, double max)
        {
            return value > min && value < max;
        }

        public static bool IsBetweenOrEqual(this double value, double min, double max)
        {
            return value >= min && value <= max;
        }

        public static bool IsBetween(this long value, long min, long max)
        {
            return value > min && value < max;
        }

        public static bool IsBetweenOrEqual(this long value, long min, long max)
        {
            return value >= min && value <= max;
        }

        public static bool IsBetween(this float value, float min, float max)
        {
            return value > min && value < max;
        }

        public static bool IsBetweenOrEqual(this float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        public static long AsLong(this decimal value)
        {
            try
            {
                return (long)value;
            }
            catch
            {
                return default(long);
            }
        }

        public static long AsLong(this double value)
        {
            try
            {
                return (long) value;
            }
            catch
            {
                return default(long);
            }
        }

        public static int AsInt(this decimal value)
        {
            try
            {
                return (int)value;
            }
            catch
            {
                return default(int);
            }
        }

        public static int AsInt(this double value)
        {
            try
            {
                return (int)value;
            }
            catch
            {
                return default(int);
            }
        }

        public static double AsDouble(this int value)
        {
            return value;
        }

        public static double AsDouble(this decimal value)
        {
            return decimal.ToDouble(value);
        }

        public static decimal AsDecimal(this int value)
        {
            return (decimal)value;
        }

        public static decimal AsDecimal(this double value)
        {
            return (decimal)value;
        }

        public static bool IsEqualWithMargin(this int value, int compareValue, int margin)
        {
            return value.IsBetweenOrEqual(compareValue * (1 - margin), compareValue * (1 + margin));
        }

        public static bool IsEqualWithMargin(this decimal value, decimal compareValue, decimal margin)
        {
            return value.IsBetweenOrEqual(compareValue * (1 - margin), compareValue * (1 + margin));
        }

        public static bool IsEqualWithMargin(this double value, double compareValue, double margin)
        {
            return value.IsBetweenOrEqual(compareValue * (1 - margin), compareValue * (1 + margin));
        }

        public static bool IsEqualWithMargin(this float value, float compareValue, float margin)
        {
            return value.IsBetweenOrEqual(compareValue * (1 - margin), compareValue * (1 + margin));
        }

        public static bool IsEqualWithMargin(this long value, long compareValue, long margin)
        {
            return value.IsBetweenOrEqual(compareValue * (1 - margin), compareValue * (1 + margin));
        }
    }
}
