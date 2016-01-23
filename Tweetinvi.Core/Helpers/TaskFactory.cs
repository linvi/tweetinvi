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
            await TaskEx.Run(action);
        }

        public async Task<T> ExecuteTaskAsync<T>(Func<T> resultFunc)
        {
            return await TaskEx.Run(resultFunc);
        }
    }
}