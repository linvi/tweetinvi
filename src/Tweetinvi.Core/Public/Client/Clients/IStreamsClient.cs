using Tweetinvi.Core.Streaming;
using Tweetinvi.Parameters;
using Tweetinvi.Streaming;

namespace Tweetinvi.Client
{
    public interface IStreamsClient
    {
        /// <inheritdoc cref="CreateSampleStream(ICreateSampleStreamParameters)"/>
        ISampleStream CreateSampleStream();

        /// <summary>
        /// Create a stream notifying that a random tweets has been created.
        /// https://dev.twitter.com/streaming/reference/get/statuses/sample
        /// </summary>
        ISampleStream CreateSampleStream(ICreateSampleStreamParameters parameters);

        /// <inheritdoc cref="CreateFilteredStream(ICreateFilteredTweetStreamParameters)"/>
        IFilteredStream CreateFilteredStream();

        /// <summary>
        /// Create a stream notifying the client when a tweet matching the specified criteria is created.
        /// https://dev.twitter.com/streaming/reference/post/statuses/filter
        /// </summary>
        IFilteredStream CreateFilteredStream(ICreateFilteredTweetStreamParameters parameters);

        /// <inheritdoc cref="CreateTweetStream(ICreateTweetStreamParameters)"/>
        ITweetStream CreateTweetStream();

        /// <summary>
        /// Create a stream that receive tweets
        /// </summary>
        ITweetStream CreateTweetStream(ICreateTweetStreamParameters parameters);

        /// <inheritdoc cref="CreateTrackedTweetStream(ICreateTrackedTweetStreamParameters)"/>
        ITrackedStream CreateTrackedTweetStream();

        /// <summary>
        /// Create a stream that receive tweets. In addition this stream allow you to filter the results received.
        /// </summary>
        ITrackedStream CreateTrackedTweetStream(ICreateTrackedTweetStreamParameters parameters);
    }
}