using System;
using System.Threading.Tasks;

namespace Tweetinvi.Core.Helpers
{
    public interface ISynchronousInvoker
    {
        T GetResultSynchronously<T>(Func<Task<T>> asyncMethod);
        void ExecuteSynchronously(Func<Task> asyncMethod);
    }

    public class SynchronousInvoker : ISynchronousInvoker
    {
        private readonly ITaskFactory _taskFactory;

        public SynchronousInvoker(ITaskFactory taskFactory)
        {
            _taskFactory = taskFactory;
        }

        public T GetResultSynchronously<T>(Func<Task<T>> asyncMethod)
        {
            Task<Task<T>> task = _taskFactory.ExecuteTaskAsync(asyncMethod);

            return task.Result.Result;
        }

        public void ExecuteSynchronously(Func<Task> asyncMethod)
        {
            Task task = _taskFactory.ExecuteTaskAsync(asyncMethod);
            task.Wait();
        }
    }
}