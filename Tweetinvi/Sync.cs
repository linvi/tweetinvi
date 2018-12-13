using System;
using System.Threading.Tasks;

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