using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public static class TaskExtensions
    {
        public static async void FireAndForget(this Task task)
        {
            await task.ConfigureAwait(false);
        }

        public static async Task<T> TimeoutAfter<T>(
            this Task<T> task,
            int timeoutInMilliseconds)
        {
            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(timeoutInMilliseconds, timeoutCancellationTokenSource.Token));
            if (completedTask == task) {
                timeoutCancellationTokenSource.Cancel();
                return await task;
            }

            throw new TimeoutException("The operation has timed out");
        }

        public static async Task TimeoutAfter(
            this Task task,
            int timeoutInMilliseconds)
        {
            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(timeoutInMilliseconds, timeoutCancellationTokenSource.Token));
            if (completedTask == task) {
                timeoutCancellationTokenSource.Cancel();
                await task;
            }

            throw new TimeoutException("The operation has timed out");
        }

        public static async Task WaitWhile(Func<bool> condition, int frequency, int timeout)
        {
            var waitTask = Task.Run(async () =>
            {
                while (condition()) await Task.Delay(frequency);
            });

            if(waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
                throw new TimeoutException();
        }

        public static async void FireAndForgetAsync<T>(
            this Task task,
            Action<T> errorHandling = null) where T : Exception
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (T exception)
            {
                errorHandling?.Invoke(exception);
            }
            catch
            {
                // ignored
            }
        }

        public static IEnumerator AsEnumerator(this Task task)
        {
            while (true)
            {
                if (task.IsFaulted)
                {
                    yield break;
                }

                if (task.IsCompleted)
                {
                    yield break;
                }

                yield return null;
            }
        }
    }
}