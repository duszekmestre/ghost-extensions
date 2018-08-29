using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Ghost.Extensions.Helpers
{
    public static class StackTraceHelper
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }
    }
}
