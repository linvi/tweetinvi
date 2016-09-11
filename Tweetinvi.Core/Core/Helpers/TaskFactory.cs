using System;
using System.Threading.Tasks;

namespace Tweetinvi.Core.Helpers
{
    public interface ITaskFactory
    {
        Task ExecuteTaskAsync(Action action);
        Task<T> ExecuteTaskAsync<T>(Func<T> resultFunc);
    }

    public class TaskFactory : ITaskFactory
    {
        public async Task ExecuteTaskAsync(Action action)
        {
#if NET_CORE
            await Task.Run(action);
#else
            await TaskEx.Run(action);
#endif
        }

        public async Task<T> ExecuteTaskAsync<T>(Func<T> resultFunc)
        {
#if NET_CORE
            return await Task.Run(resultFunc);
#else
            return await TaskEx.Run(resultFunc);
#endif
        }
    }
}