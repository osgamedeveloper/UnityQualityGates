using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Executes a task without waiting for its completion and suppresses any exceptions.
        /// </summary>
        public static async void FireAndForget(this Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled exception in FireAndForget: {ex}");
            }
        }

        /// <summary>
        /// Sets a timeout for a task and throws a TimeoutException if the task does not complete in time.
        /// </summary>
        public static async Task<T> TimeoutAfter<T>(this Task<T> task, int timeoutInMilliseconds)
        {
            using var cts = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(timeoutInMilliseconds, cts.Token));
            if (completedTask == task)
            {
                cts.Cancel();
                return await task;
            }

            throw new TimeoutException("The operation has timed out");
        }

        /// <summary>
        /// Sets a timeout for a non-generic task and throws a TimeoutException if the task does not complete in time.
        /// </summary>
        public static async Task TimeoutAfter(this Task task, int timeoutInMilliseconds)
        {
            using var cts = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(timeoutInMilliseconds, cts.Token));
            if (completedTask == task)
            {
                cts.Cancel();
                await task;
                return;
            }

            throw new TimeoutException("The operation has timed out");
        }

        /// <summary>
        /// Waits while a condition is true, with a specified check frequency and timeout.
        /// </summary>
        public static async Task WaitWhile(Func<bool> condition, int frequency, int timeout)
        {
            var waitTask = Task.Run(async () =>
            {
                while (condition())
                    await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
                throw new TimeoutException("WaitWhile timed out.");
        }

        /// <summary>
        /// Executes a task asynchronously and handles exceptions using the provided handler.
        /// </summary>
        public static async void FireAndForgetAsync<T>(this Task task, Action<T> errorHandling = null) where T : Exception
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (T exception)
            {
                errorHandling?.Invoke(exception);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled exception in FireAndForgetAsync: {ex}");
            }
        }

        /// <summary>
        /// Converts a Task into an IEnumerator for use in Unity coroutines.
        /// </summary>
        public static IEnumerator AsEnumerator(this Task task)
        {
            while (!task.IsCompleted)
                yield return null;

            if (task.IsFaulted)
                throw task.Exception ?? new Exception("Task failed without an exception.");
        }
    }
}