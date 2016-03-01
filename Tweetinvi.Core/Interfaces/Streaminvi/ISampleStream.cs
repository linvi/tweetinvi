using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Events.EventArguments;

namespace Tweetinvi.Core.Interfaces.Streaminvi
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