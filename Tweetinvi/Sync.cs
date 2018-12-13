using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        public static ConfiguredTaskAwaitable ExecuteTaskAsync(Action action)
        {
            prepareExecutionContext();

            return Task.Run(action).ConfigureAwait(false);
        }

        /// <summary>
        /// Execute a task asynchronously with Tweetinvi
        /// </summary>
        public static ConfiguredTaskAwaitable<T> ExecuteTaskAsync<T>(Func<T> resultFunc)
        {
            prepareExecutionContext();

            return Task.Run(resultFunc).ConfigureAwait(false);
        }

        /// <summary>
        /// Prepare the execution context for copying.
        /// Any objects in the EC whose (modified) values we want to be available to the calling thread
        /// need to be instantiated before copying the EC.
        /// </summary>
        private static void prepareExecutionContext()
        {
            // Ensure any objects on the execution context that we want to update for the calling thread
            //  are instantiated before we copy the execution context.
            ICrossExecutionContextPreparer ecPreparer = TweetinviContainer.Resolve<ICrossExecutionContextPreparer>();
            ecPreparer.Prepare();
        }
    }
}