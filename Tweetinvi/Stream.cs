using System;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Credentials;
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

        [ThreadStatic]
        private static ICredentialsAccessor _credentialsAccessor;
        private static ICredentialsAccessor ThreadCredentialsAccessor
        {
            get
            {
                if (_credentialsAccessor == null)
                {
                    _credentialsAccessor = TweetinviContainer.Resolve<ICredentialsAccessor>();
                }

                return _credentialsAccessor;
            }
        }

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
        public static ITweetStream CreateTweetStream(ITwitterCredentials credentials = null)
        {
            var stream = _tweetStreamUnityFactory.Create();
            stream.Credentials = credentials ?? ThreadCredentialsAccessor.CurrentThreadCredentials;
            return stream;
        }

        /// <summary>
        /// Create a stream that receive tweets. In addition this stream allow you to filter the results received.
        /// </summary>
        public static ITrackedStream CreateTrackedStream(ITwitterCredentials credentials = null)
        {
            var stream = _trackedStreamUnityFactory.Create();
            stream.Credentials = credentials ?? ThreadCredentialsAccessor.CurrentThreadCredentials;
            return stream;
        }

        /// <summary>
        /// Create a stream notifying that a random tweets has been created.
        /// https://dev.twitter.com/streaming/reference/get/statuses/sample
        /// </summary>
        public static ISampleStream CreateSampleStream(ITwitterCredentials credentials = null)
        {
            var stream = _sampleStreamUnityFactory.Create();
            stream.Credentials = credentials ?? ThreadCredentialsAccessor.CurrentThreadCredentials;
            return stream;
        }

        /// <summary>
        /// Create a stream notifying the client when a tweet matching the specified criteria is created.
        /// https://dev.twitter.com/streaming/reference/post/statuses/filter
        /// </summary>
        public static IFilteredStream CreateFilteredStream(ITwitterCredentials credentials = null)
        {
            var stream = _filteredStreamUnityFactory.Create();
            stream.Credentials = credentials ?? ThreadCredentialsAccessor.CurrentThreadCredentials;
            return stream;
        }

        /// <summary>
        /// Create a stream notifying the client about everything that can happen to a user.
        /// https://dev.twitter.com/streaming/reference/get/user
        /// </summary>
        public static IUserStream CreateUserStream(ITwitterCredentials credentials = null)
        {
            var stream = _userStreamFactory.Create();
            stream.Credentials = credentials ?? ThreadCredentialsAccessor.CurrentThreadCredentials;
            return stream;
        }
    }
}