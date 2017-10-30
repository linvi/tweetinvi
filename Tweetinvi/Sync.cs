using Nito.AsyncEx;
using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Helpers;

namespace Tweetinvi
{
    /// <summary>
    /// Async made easy.
    /// </summary>
    public static class Sync
    {
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

        // ALERT : THIS CODE IS AWESOME :D

        /// <summary>
        /// Execute a task asynchronously with Tweetinvi
        /// </summary>
        public static async Task ExecuteTaskAsync(Action action)
        {
            // We store the credentials at the time of the call within the local memory
            var credentialsAtInvokeTime = CredentialsAccessor.CurrentThreadCredentials;

            // We are cloning to avoid changes to the settings before the async operation starts
            var sourceThreadSettingsClone = TweetinviConfig.CurrentThreadSettings.Clone();

            // The lambda expression will store 'credentialsAtInvokeTime' within a generated class
            // In order to keep the reference to the credentials at the time of invocation
            var operationRunWithSpecificCredentials = new Action(() =>
            {
                TweetinviConfig.CurrentThreadSettings.InitialiseFrom(sourceThreadSettingsClone);

                // We get the newly created credentialsAccessor for the async thread (CredentialsAccessor are Thread specific)
                var credentialsAccessor = TweetinviContainer.Resolve<ICredentialsAccessor>();

                // We now use credentials of the lambda expression local variables to perform our operation
                credentialsAccessor.ExecuteOperationWithCredentials(credentialsAtInvokeTime, action);
            });

            AsyncContext.Run(async () =>
            {
                await _taskFactory.ExecuteTaskAsync(operationRunWithSpecificCredentials);
            });

            await Task.CompletedTask;
        }

        // ALERT : THIS CODE IS AWESOME :D

        /// <summary>
        /// Execute a task asynchronously with Tweetinvi
        /// </summary>
        public static async Task<T> ExecuteTaskAsync<T>(Func<T> resultFunc)
        {
            // We store the credentials at the time of the call within the local memory
            var credentialsAtInvokeTime = CredentialsAccessor.CurrentThreadCredentials;

            // We are cloning to avoid changes to the settings before the async operation starts
            var sourceThreadSettingsClone = TweetinviConfig.CurrentThreadSettings.Clone();

            // The lambda expression will store 'credentialsAtInvokeTime' within a generated class
            // In order to keep the reference to the credentials at the time of invocation
            var operationRunWithSpecificCredentials = new Func<T>(() =>
            {
                TweetinviConfig.CurrentThreadSettings.InitialiseFrom(sourceThreadSettingsClone);

                // We get the newly created credentialsAccessor for the async thread (CredentialsAccessor are Thread specific)
                var credentialsAccessor = TweetinviContainer.Resolve<ICredentialsAccessor>();

                // We now use credentials of the lambda expression local variables to perform our operation
                //return AsyncContext.Run(() => credentialsAccessor.ExecuteOperationWithCredentials(credentialsAtInvokeTime, resultFunc));
                return credentialsAccessor.ExecuteOperationWithCredentials(credentialsAtInvokeTime, resultFunc);
            });

            var result = AsyncContext.Run(async () =>
            {
                return await _taskFactory.ExecuteTaskAsync(operationRunWithSpecificCredentials);
            });

            return await Task.FromResult(result);
        }
    }
}