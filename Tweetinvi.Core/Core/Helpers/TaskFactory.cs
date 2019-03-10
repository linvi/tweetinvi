using System;
using System.Threading.Tasks;
using Tweetinvi.Core.ExecutionContext;

namespace Tweetinvi.Core.Helpers
{
    public interface ITaskFactory
    {
        Task InitializeAsyncContextAndExecute(Action action);
        Task<T> InitializeAsyncContextAndExecute<T>(Func<T> resultFunc);
    }

    public class TaskFactory : ITaskFactory
    {
        private readonly IAsyncContextPreparer _asyncContextPreparer;

        public TaskFactory(IAsyncContextPreparer asyncContextPreparer)
        {
            _asyncContextPreparer = asyncContextPreparer;
        }

        public Task InitializeAsyncContextAndExecute(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _asyncContextPreparer.PrepareFromParentAsyncContext();

            return Task.Run(() =>
            {
                _asyncContextPreparer.PrepareFromChildAsyncContext();
                action();
            });
        }

        public Task<T> InitializeAsyncContextAndExecute<T>(Func<T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            _asyncContextPreparer.PrepareFromParentAsyncContext();

            return Task.Run(() =>
            {
                _asyncContextPreparer.PrepareFromChildAsyncContext();

                var operationResult = func();
                return operationResult;
            });
        }
    }
}