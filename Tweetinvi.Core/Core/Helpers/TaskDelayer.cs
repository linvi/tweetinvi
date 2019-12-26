using System;
using System.Threading.Tasks;

namespace Tweetinvi.Core.Helpers
{
    public interface ITaskDelayer
    {
        Task Delay(TimeSpan timeSpan);
    }

    public class TaskDelayer : ITaskDelayer
    {
        public Task Delay(TimeSpan timeSpan)
        {
            return Task.Delay(timeSpan);
        }
    }
}