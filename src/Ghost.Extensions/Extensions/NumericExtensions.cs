namespace Ghost.Extensions.Extensions
{
    public static class NumericExtensions
    {
        public static bool Between(this int value, int min, int max)
        {
            return value > min && value < max;
        }

        public static bool BetweenOrEqual(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        public static bool Between(this decimal value, decimal min, decimal max)
        {
            return value > min && value < max;
        }

        public static bool BetweenOrEqual(this decimal value, decimal min, decimal max)
        {
            return value >= min && value <= max;
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

        public static double AsDouble(this int value)
        {
            return value;
        }

        public static double AsDouble(this decimal value)
        {
            return decimal.ToDouble(value);
        }

        public static decimal AsDecimal(this double value)
        {
            return (decimal)value;
        }

        public static bool Between(this double value, double min, double max)
        {
            return value > min && value < max;
        }

        public static bool BetweenOrEqual(this double value, double min, double max)
        {
            return value >= min && value <= max;
        }

        public static bool Between(this long value, long min, long max)
        {
            return value > min && value < max;
        }

        public static bool BetweenOrEqual(this long value, long min, long max)
        {
            return value >= min && value <= max;
        }

        public static bool Between(this float value, float min, float max)
        {
            return value > min && value < max;
        }

        public static bool BetweenOrEqual(this float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        public static bool EqualWithMargin(this int value, int compareValue, int margin)
        {
            return value.BetweenOrEqual(compareValue * (1 - margin), compareValue * (1 + margin));
        }

        public static bool EqualWithMargin(this decimal value, decimal compareValue, decimal margin)
        {
            return value.BetweenOrEqual(compareValue * (1 - margin), compareValue * (1 + margin));
        }

        public static bool EqualWithMargin(this double value, double compareValue, double margin)
        {
            return value.BetweenOrEqual(compareValue * (1 - margin), compareValue * (1 + margin));
        }

        public static bool EqualWithMargin(this float value, float compareValue, float margin)
        {
            return value.BetweenOrEqual(compareValue * (1 - margin), compareValue * (1 + margin));
        }

        public static bool EqualWithMargin(this long value, long compareValue, long margin)
        {
            return value.BetweenOrEqual(compareValue * (1 - margin), compareValue * (1 + margin));
        }
    }
}
