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
        private readonly ICrossExecutionContextPreparer _crossExecutionContextPreparer;

        public TaskFactory(ICrossExecutionContextPreparer crossExecutionContextPreparer)
        {
            _crossExecutionContextPreparer = crossExecutionContextPreparer;
        }

        public Task ExecuteTaskAsync(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _crossExecutionContextPreparer.Prepare();

            return Task.Run(action);
        }

        public Task<T> ExecuteTaskAsync<T>(Func<T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            _crossExecutionContextPreparer.Prepare();

            return Task.Run(func);
        }
    }
}