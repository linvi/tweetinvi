using System;
using Tweetinvi.Events;

namespace Tweetinvi.Core.Streaming.V2
{
    public interface ITweetStreamV2<T>
    {
        /// <summary>
        /// Any event message received
        /// </summary>
        event EventHandler<StreamEventReceivedArgs> EventReceived;

        /// <summary>
        /// A tweet has been received.
        /// </summary>
        event EventHandler<T> TweetReceived;

        /// <summary>
        /// Stop running the stream
        /// </summary>
        void StopStream();
    }
}