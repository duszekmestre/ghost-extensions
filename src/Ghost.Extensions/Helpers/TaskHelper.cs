using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ghost.Extensions.Helpers
{
    public class TaskHelper
    {
        public static async Task For(int from, int to, Func<int, Task> action)
        {
            await Task.WhenAll(Enumerable.Range(from, to - from).Select(action.Invoke));
        }
    }
}
