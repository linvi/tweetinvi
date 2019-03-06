using System;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.ExecutionContext;
using Tweetinvi.Core.Helpers;

namespace Tweetinvi
{
    /// <summary>
    /// Async made easy.
    /// </summary>
    public static class Sync
    {
        private static IAsyncContextPreparer _asyncContextPreparer;
        private static ITaskFactory _taskFactory;

        static Sync()
        {
            _asyncContextPreparer = TweetinviContainer.Resolve<IAsyncContextPreparer>();
            _taskFactory = TweetinviContainer.Resolve<ITaskFactory>();
        }

        /// <summary>
        /// Execute a task asynchronously with Tweetinvi
        /// </summary>
        public static Task ExecuteTaskAsync(Action action)
        {
            return _taskFactory.ExecuteTaskAsync(action);
        }

        /// <summary>
        /// Execute a task asynchronously with Tweetinvi
        /// </summary>
        public static Task<T> ExecuteTaskAsync<T>(Func<T> func)
        {
            return _taskFactory.ExecuteTaskAsync(func);
        }

        /// <summary>
        /// PrepareAsyncContext the current Task for Tweetinvi to be used asynchronously within it
        /// </summary>
        public static void PrepareForAsync()
        {
            _asyncContextPreparer.PrepareAsyncContext();
        }

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