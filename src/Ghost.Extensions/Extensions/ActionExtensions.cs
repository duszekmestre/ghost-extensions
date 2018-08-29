using System;
using System.Threading.Tasks;

namespace Ghost.Extensions.Extensions
{
    public static class ActionExtensions
    {
        public static TOutput Repeat<TOutput>(this Func<TOutput> action,
            int attempts,
            Action postLoopAction = null,
            bool throwLastException = false,
            TOutput defaultOutput = default(TOutput))
        {
            do
            {

                try
                {
                    return action();
                }
                catch
                {
                    if (throwLastException && attempts <= 1)
                    {
                        throw;  //the last attemt failed, so we rethrow the exception
                    }
                }
                finally
                {
                    attempts--;
                }

                postLoopAction?.Invoke();
            } while (attempts > 0);

            return defaultOutput;
        }

        public static async Task<TOutput> RepeatAsync<TOutput>(
            this Func<Task<TOutput>> action,
            int attempts,
            Action postLoopAction = null,
            bool throwLastException = false,
            TOutput defaultOutput = default(TOutput))
        {
            do
            {

                try
                {
                    return await action();
                }
                catch (Exception)
                {
                    if (throwLastException && attempts <= 1)
                    {
                        throw;  //the last attemt failed, so we rethrow the exception
                    }
                }
                finally
                {
                    attempts--;
                }

                postLoopAction?.Invoke();
            } while (attempts > 0);

            return defaultOutput;
        }

        public static TOutput RepeatIf<TOutput>(this Func<TOutput> action,
            Func<Exception, bool> repeatPredicate,
            int attempts,
            bool throwLastException = false,
            TOutput defaultOutput = default(TOutput))
        {
            do
            {

                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    if (!repeatPredicate(ex))
                    {
                        if (throwLastException)
                        {
                            throw;
                        }

                        return defaultOutput;
                    }

                    if (throwLastException && attempts <= 1)
                    {
                        throw;  //the last attemt failed, so we rethrow the exception
                    }
                }
                finally
                {
                    attempts--;
                }
            } while (attempts > 0);

            return defaultOutput;
        }

        public static async Task<TOutput> RepeatIfAsync<TOutput>(this Func<Task<TOutput>> action,
            Func<Exception, bool> repeatPredicate,
            int attempts,
            bool throwLastException = false,
            TOutput defaultOutput = default(TOutput))
        {
            do
            {

                try
                {
                    return await action();
                }
                catch (Exception ex)
                {
                    if (!repeatPredicate(ex))
                    {
                        if (throwLastException)
                        {
                            throw;
                        }

                        return defaultOutput;
                    }

                    if (throwLastException && attempts <= 1)
                    {
                        throw;  //the last attemt failed, so we rethrow the exception
                    }
                }
                finally
                {
                    attempts--;
                }
            } while (attempts > 0);

            return defaultOutput;
        }
    }
}
