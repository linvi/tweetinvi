using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Events.EventArguments;

namespace Tweetinvi.Core.Interfaces.Streaminvi
{
    public interface ITrackedStream : ITwitterStream, ITrackableStream<ITweet>
    {
        /// <summary>
        /// A tweet matching the specified filters has been received.
        /// </summary>
        event EventHandler<MatchedTweetReceivedEventArgs> MatchingTweetReceived;

        /// <summary>
        /// A tweet has been received, regardless of the fact that is matching the specified criteria.
        /// </summary>
        event EventHandler<MatchedTweetReceivedEventArgs> TweetReceived;

        /// <summary>
        /// A tweet has been received but it does not match all of the specified filters.
        /// </summary>
        event EventHandler<TweetEventArgs> NonMatchingTweetReceived;

        /// <summary>
        /// Start a stream SYNCHRONOUSLY. The thread will continue after the stream has stopped.
        /// </summary>
        void StartStream(string url);

        /// <summary>
        /// Start a stream ASYNCHRONOUSLY. The task will complete when the stream stops.
        /// </summary>
        Task StartStreamAsync(string url);
    }
}