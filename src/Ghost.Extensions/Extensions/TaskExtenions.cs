using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Ghost.Extensions.Extensions
{
    public static class TaskExtenions
    {
        public static void WaitSynch(this Task task)
        {
            try
            {
                task.Wait();
            }
            catch (Exception ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
        }

        public static T WaitSynch<T>(this Task<T> task)
        {
            try
            {
                task.Wait();
            }
            catch (Exception ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();                
            }

            return task.Result;
        }


        public static async Task WaitAsync(this Task task)
        {
            var thrownException = await GetThrownExceptionFromTask(task);
            ThrowProperExceptionFromTask(thrownException);
        }

        public static async Task<T> WaitAsync<T>(this Task<T> task)
        {
            var thrownException = await GetThrownExceptionFromTask(task);
            ThrowProperExceptionFromTask(thrownException);

            return task.Result;
        }

        private static async Task<Exception> GetThrownExceptionFromTask(Task task)
        {
            return await Task.Run(() =>
            {
                try
                {
                    task.Wait();
                    return null;
                }
                catch (AggregateException exception)
                {
                    return GetClearExceptionfromAggregatedException(exception);
                }
            });
        }

        private static Exception GetClearExceptionfromAggregatedException(AggregateException exception)
        {
            if (exception.InnerExceptions.NullOrEmpty() || exception.InnerExceptions.Count > 1)
            {
                return exception;
            }

            return exception.InnerException ?? exception;
        }

        private static void ThrowProperExceptionFromTask(Exception thrownException)
        {
            if (thrownException == null)
            {
                return;
            }

            var aggregateException = thrownException as AggregateException;
            if (aggregateException != null)
            {
                throw aggregateException.Flatten();
            }

            throw thrownException;
        }
              

        public static async Task<TResult> WaitAsyncWithException<TResult, TException>(this Task<TResult> task)
            where TException : Exception
        {
            try
            {
                return await task.WaitAsync();
            }
            catch (AggregateException exception)
            {
                var expectedException = exception.InnerExceptions.OfType<TException>().FirstOrDefault();

                if (expectedException != null)
                {
                    throw expectedException;
                }

                throw;
            }
        }

        public static void RunWithExceptions(Action taskAction, Action<Exception> exceptionHandler = null)
        {
            if (taskAction == null)
            {
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    taskAction.Invoke();
                }
                catch (Exception exception)
                {
                    exceptionHandler?.Invoke(exception);
                }
            });
        }
    }
}