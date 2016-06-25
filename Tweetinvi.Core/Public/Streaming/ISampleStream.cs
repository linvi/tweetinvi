using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Events;

namespace Tweetinvi.Streaming
{
    public interface ISampleStream : ITwitterStream
    {
        /// <summary>
        /// A tweet has been received.
        /// </summary>
        event EventHandler<TweetReceivedEventArgs> TweetReceived;

        /// <summary>
        /// Start a stream SYNCHRONOUSLY. The thread will continue after the stream has stopped.
        /// </summary>
        void StartStream();

        /// <summary>
        /// Start a stream ASYNCHRONOUSLY. The task will complete when the stream stops.
        /// </summary>
        Task StartStreamAsync();
    }
}