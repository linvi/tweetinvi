using System;
using System.Threading.Tasks;
using Tweetinvi.Core.ExecutionContext;

namespace Tweetinvi.Core.Helpers
{
    public interface ITaskFactory
    {
        Task ExecuteTaskAsync(Action action);
        Task<T> ExecuteTaskAsync<T>(Func<T> resultFunc);
    }

    public class TaskFactory : ITaskFactory
    {
        private readonly IAsyncContextPreparer _asyncContextPreparer;

        public TaskFactory(IAsyncContextPreparer asyncContextPreparer)
        {
            _asyncContextPreparer = asyncContextPreparer;
        }

        public Task ExecuteTaskAsync(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _asyncContextPreparer.PrepareAsyncContext();

            return Task.Run(action);
        }

        public Task<T> ExecuteTaskAsync<T>(Func<T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            _asyncContextPreparer.PrepareAsyncContext();

            return Task.Run(func);
        }
    }
}