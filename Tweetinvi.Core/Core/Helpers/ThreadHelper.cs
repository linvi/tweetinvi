using System.Threading.Tasks;

namespace Tweetinvi.Core.Helpers
{
    public interface IThreadHelper
    {
        void Sleep(int milliseconds);
    }

    public class ThreadHelper : IThreadHelper
    {
        public void Sleep(int milliseconds)
        {
            if (milliseconds > 0)
            {
#if NET_CORE
                Task.Delay(milliseconds).Wait();
#else
                TaskEx.Delay(milliseconds).Wait();
#endif
            }
        }
    }
}