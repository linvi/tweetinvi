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
#if NET_CORE
            Task<T> task = Task.Run(() => asyncMethod());
#else
            Task<T> task = TaskEx.Run(() => asyncMethod());
#endif
            task.Wait();
            return task.Result;
        }

        public void ExecuteSynchronously(Func<Task> asyncMethod)
        {
#if NET_CORE
            Task task = Task.Run(() => asyncMethod());
#else
            Task task = TaskEx.Run(() => asyncMethod());
#endif
            task.Wait();
        }
    }
}