using System.Threading.Tasks;
using Tweetinvi.Core.Streaming.V2;
using Tweetinvi.Events.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Streaming.V2
{
    public interface IFilteredStreamV2 : ITweetStreamV2<FilteredStreamTweetV2EventArgs>
    {
        /// <inheritdoc cref="StartAsync(IStartFilteredStreamV2Parameters)"/>
        Task StartAsync();

        /// <summary>
        /// Start the stream asynchronously. The task will complete when the stream stops.
        /// </summary>
        Task StartAsync(IStartFilteredStreamV2Parameters parameters);
    }
}