using System;
using System.Threading.Tasks;

namespace Tweetinvi.Core.Helpers
{
    public interface ISynchronousInvoker
    {
        T GetResultSyncrhonously<T>(Func<Task<T>> asyncMethod);
        void ExecuteSynchronously(Func<Task> asyncMethod);
    }

    public class SynchronousInvoker : ISynchronousInvoker
    {
        public T GetResultSyncrhonously<T>(Func<Task<T>> asyncMethod)
        {
            Task<T> task = TaskEx.Run(() => asyncMethod());
            task.Wait();
            return task.Result;
        }

        public void ExecuteSynchronously(Func<Task> asyncMethod)
        {
            Task task = TaskEx.Run(() => asyncMethod());
            task.Wait();
        }
    }
}