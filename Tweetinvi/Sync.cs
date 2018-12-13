using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;

namespace Tweetinvi
{
    /// <summary>
    /// Async made easy.
    /// </summary>
    public static class Sync
    {
        public static async Task<T> RunAsync<T>(Func<T> func)
        {
            using (var thread = new AsyncContextThread())
            {
                var result = await thread.Factory.Run(() => { return func(); });
                return result;
            }
        }

        private static readonly ITaskFactory _taskFactory;

        [ThreadStatic]
        private static ICredentialsAccessor _credentialsAccessor;

        /// <summary>
        /// Object storing the current thread credentials
        /// </summary>
        public static ICredentialsAccessor CredentialsAccessor
        {
            get
            {
                if (_credentialsAccessor == null)
                {
                    InitializeStaticThread();
                }

                return _credentialsAccessor;
            }
        }

        static Sync()
        {
            InitializeStaticThread();

            _taskFactory = TweetinviContainer.Resolve<ITaskFactory>();
        }

        private static void InitializeStaticThread()
        {
            _credentialsAccessor = TweetinviContainer.Resolve<ICredentialsAccessor>();
        }

        /// <summary>
        /// Execute a task asynchronously with Tweetinvi
        /// </summary>
        public static async Task ExecuteTaskAsync(Action action)
        {
            // Use the implementation for Func<T> so that it only needs to be maintained in one place
            await ExecuteTaskAsync(() =>
            {
                action();
                return 0;
            });
        }

        /// <summary>
        /// Execute a task asynchronously with Tweetinvi
        /// </summary>
        public static async Task<T> ExecuteTaskAsync<T>(Func<T> resultFunc)
        {
            // TODO: Ensure any objects on the execution context that we want to update for the calling thread
            //  are instantiated before we copy the EC.
            return await Task.Run(resultFunc).ConfigureAwait(false);
        }
    }
}