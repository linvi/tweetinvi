using System;
using System.Threading.Tasks;
using Tweetinvi.Events;

namespace Tweetinvi.Core.Streaming
{
    public interface ITweetStream : ITwitterStream
    {
        /// <summary>
        /// Event informing that a tweet has been received.
        /// </summary>
        event EventHandler<TweetReceivedEventArgs> TweetReceived;

        /// <summary>
        /// Start a stream SYNCHRONOUSLY. The thread will continue after the stream has stopped.
        /// </summary>
        Task StartStream(string url);
    }
}