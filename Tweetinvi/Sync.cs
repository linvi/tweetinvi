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
            TweetinviContainer.Resolve<ICrossExecutionContextPreparer>().Prepare();

            return Task.Run(action).ConfigureAwait(false);
        }

        /// <summary>
        /// Execute a task asynchronously with Tweetinvi
        /// </summary>
        public static ConfiguredTaskAwaitable<T> ExecuteTaskAsync<T>(Func<T> resultFunc)
        {
            TweetinviContainer.Resolve<ICrossExecutionContextPreparer>().Prepare();

            return Task.Run(resultFunc).ConfigureAwait(false);
        }
    }
}