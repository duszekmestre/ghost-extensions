using System;

namespace Ghost.Extensions.Helpers
{
    public class ClearStopper
    {
        public DateTime StartDate { get; set; }

        public ClearStopper()
        {
            this.StartDate = DateTime.UtcNow;
        }

        public TimeSpan Stop()
        {
            return DateTime.UtcNow - StartDate;
        }
    }
}
