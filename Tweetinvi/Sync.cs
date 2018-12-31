using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.ExecutionContext;

namespace Tweetinvi
{
    /// <summary>
    /// Async made easy.
    /// </summary>
    public static class Sync
    {
        /// <summary>
        /// Execute a task asynchronously with Tweetinvi
        /// </summary>
        public static Task ExecuteTaskAsync(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            PrepareForAsync();

            return Task.Run(action);
        }

        /// <summary>
        /// Execute a task asynchronously with Tweetinvi
        /// </summary>
        public static Task<T> ExecuteTaskAsync<T>(Func<T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            PrepareForAsync();

            return Task.Run(func);
        }

        /// <summary>
        /// Prepare the current Task for Tweetinvi to be used asynchronously within it
        /// </summary>
        public static void PrepareForAsync() => TweetinviContainer.Resolve<ICrossExecutionContextPreparer>().Prepare();

        /// <summary>
        /// Execute a task asynchronously with Tweetinvi independently of the calling context
        /// </summary>
        public static Task ExecuteIsolatedTaskAsync(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // Prevent execution context from being passed over to the new thread
            Task t;
            using (ExecutionContext.SuppressFlow())
            {
                t = Task.Run(action);
            }
            return t;
        }

        /// <summary>
        /// Execute a task asynchronously with Tweetinvi independently of the calling context
        /// </summary>
        public static Task<T> ExecuteIsolatedTaskAsync<T>(Func<T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            // Prevent execution context from being passed over to the new thread
            Task<T> t;
            using (ExecutionContext.SuppressFlow())
            {
                t = Task.Run(func);
            }
            return t;
        }
    }
}