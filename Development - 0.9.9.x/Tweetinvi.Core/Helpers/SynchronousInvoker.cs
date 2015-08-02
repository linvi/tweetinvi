using System.Threading.Tasks;

namespace Tweetinvi.Core.Helpers
{
    public interface ISynchronousInvoker
    {
        T GetResultSyncrhonously<T>(Task<T> task);
        void ExecuteSynchronously(Task task);
    }

    public class SynchronousInvoker : ISynchronousInvoker
    {
        public T GetResultSyncrhonously<T>(Task<T> task)
        {
            task.Wait();
            return task.Result;
        }

        public void ExecuteSynchronously(Task task)
        {
            task.Wait();
        }
    }
}