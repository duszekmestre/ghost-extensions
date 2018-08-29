using Ghost.Extensions.Extensions;
using System;

namespace Ghost.Extensions.Helpers
{
    public static class DateTimeHelper
    {
        public static int DaysDifference(this DateTime from, DateTime to)
        {
            if (to.Date <= from.Date)
            {
                return 0;
            }

            return Math.Ceiling((to.Date - from.Date).TotalDays).AsInt();
        }
    }
}
