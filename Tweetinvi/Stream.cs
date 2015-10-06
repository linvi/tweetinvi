using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Streaminvi;

namespace Tweetinvi
{
    public static class Stream
    {
        private static readonly IFactory<IUserStream> _userStreamFactory;
        private static readonly IFactory<ITweetStream> _tweetStreamUnityFactory;
        private static readonly IFactory<ISampleStream> _sampleStreamUnityFactory;
        private static readonly IFactory<ITrackedStream> _trackedStreamUnityFactory;
        private static readonly IFactory<IFilteredStream> _filteredStreamUnityFactory;

        static Stream()
        {
            _userStreamFactory = TweetinviContainer.Resolve<IFactory<IUserStream>>();
            _tweetStreamUnityFactory = TweetinviContainer.Resolve<IFactory<ITweetStream>>();
            _sampleStreamUnityFactory = TweetinviContainer.Resolve<IFactory<ISampleStream>>();
            _trackedStreamUnityFactory = TweetinviContainer.Resolve<IFactory<ITrackedStream>>();
            _filteredStreamUnityFactory = TweetinviContainer.Resolve<IFactory<IFilteredStream>>();
        }

        /// <summary>
        /// Create a stream that receive tweets
        /// </summary>
        public static ITweetStream CreateTweetStream()
        {
            return _tweetStreamUnityFactory.Create();
        }

        /// <summary>
        /// Create a stream that receive tweets. In addition this stream allow you to filter the results received.
        /// </summary>
        public static ITrackedStream CreateTrackedStream()
        {
            return _trackedStreamUnityFactory.Create();
        }

        /// <summary>
        /// Create a a stream that get the tweets from the Twitter public Sample stream
        /// https://dev.twitter.com/streaming/reference/get/statuses/sample
        /// </summary>
        public static ISampleStream CreateSampleStream()
        {
            return _sampleStreamUnityFactory.Create();
        }

        /// <summary>
        /// Create a a stream that get the tweets from the Twitter public Sample stream
        /// https://dev.twitter.com/streaming/reference/post/statuses/filter
        /// </summary>
        public static IFilteredStream CreateFilteredStream()
        {
            return _filteredStreamUnityFactory.Create();
        }

        /// <summary>
        /// Create a a stream that get the tweets from the Twitter public Sample stream
        /// https://dev.twitter.com/streaming/reference/get/user
        /// </summary>
        public static IUserStream CreateUserStream()
        {
            return _userStreamFactory.Create();
        }
    }
}