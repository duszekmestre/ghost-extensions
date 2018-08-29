using System;
using System.Diagnostics;

namespace Ghost.Extensions.Helpers
{
    public class Stopper : IDisposable
    {
        private readonly string message;
        private readonly Action<string> output;

        public DateTime StartDate { get; set; }

        public Stopper()
            : this(string.Empty)
        {
        }

        public Stopper(string message, Action<string> output = null, bool logOnStart = false)
        {
            this.message = message;
            this.output = output ?? ((msg) => Debug.WriteLine(msg));

            if (logOnStart)
            {
                var formattedMessage = $"{nameof(Stopper).ToUpper()} | {this.message} - START";

                output(formattedMessage);
            }

            this.StartDate = DateTime.UtcNow;
        }

        public void Dispose()
        {
            Stop(this.message);
        }

        public void Stop()
        {
            this.Stop(this.message);
        }

        public void Stop(string msg)
        {
            var endTime = DateTime.UtcNow;
            double time = (endTime - StartDate).TotalMilliseconds;
            var formattedMessage = $"{nameof(Stopper).ToUpper()} | {msg} - {time}ms";

            output(formattedMessage);

            this.StartDate = DateTime.UtcNow;
        }
    }

    public static class StopperExtensions
    {
        public static void WithStoper(this Action action, string message = "")
        {
            using (new Stopper(message))
            {
                action.Invoke();
            }
        }

        public static TOut WithStoper<TOut>(this Func<TOut> func, string message = "")
        {
            using (new Stopper(message))
            {
                return func.Invoke();
            }
        }
    }
}
