using System;
using System.Threading.Tasks;
using Tweetinvi.Events.V2;
using Tweetinvi.Parameters.StreamsV2Client;

namespace Tweetinvi.Streaming.V2
{
    public interface ISampleStreamV2
    {
        /// <summary>
        /// A tweet has been received.
        /// </summary>
        event EventHandler<TweetV2ReceivedEventArgs> TweetReceived;

        /// <inheritdoc cref="StartAsync(IStartSampleStreamV2Parameters)"/>
        Task StartAsync();

        /// <summary>
        /// Start a stream ASYNCHRONOUSLY. The task will complete when the stream stops.
        /// </summary>
        Task StartAsync(IStartSampleStreamV2Parameters parameters);
    }
}