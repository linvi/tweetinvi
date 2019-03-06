using System;
using Tweetinvi.Core;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Public.Streaming;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace Tweetinvi
{
    /// <summary>
    /// Access Twitter live feeds.
    /// </summary>
    public static class Stream
    {
        private static readonly IFactory<ITweetStream> _tweetStreamUnityFactory;
        private static readonly IFactory<ISampleStream> _sampleStreamUnityFactory;
        private static readonly IFactory<ITrackedStream> _trackedStreamUnityFactory;
        private static readonly IFactory<IFilteredStream> _filteredStreamUnityFactory;
        private static readonly IFactory<IAccountActivityStream> _accountActivityStreamFactory;

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

        [ThreadStatic]
        private static ITweetinviSettingsAccessor _threadSettingsAccessor;
        private static ITweetinviSettingsAccessor ThreadSettingsAccessor
        {
            get
            {
                if (_threadSettingsAccessor == null)
                {
                    _threadSettingsAccessor = TweetinviContainer.Resolve<ITweetinviSettingsAccessor>();
                }

                return _threadSettingsAccessor;
            }
        }

        static Stream()
        {
            _tweetStreamUnityFactory = TweetinviContainer.Resolve<IFactory<ITweetStream>>();
            _sampleStreamUnityFactory = TweetinviContainer.Resolve<IFactory<ISampleStream>>();
            _trackedStreamUnityFactory = TweetinviContainer.Resolve<IFactory<ITrackedStream>>();
            _filteredStreamUnityFactory = TweetinviContainer.Resolve<IFactory<IFilteredStream>>();
            _accountActivityStreamFactory = TweetinviContainer.Resolve<IFactory<IAccountActivityStream>>();
        }

        /// <summary>
        /// Create a stream that receive tweets
        /// </summary>
        public static ITweetStream CreateTweetStream(ITwitterCredentials credentials = null, TweetMode? tweetMode = null)
        {
            return GetConfiguredStream(_tweetStreamUnityFactory.Create(), credentials, tweetMode);
        }

        /// <summary>
        /// Create a stream that receive tweets. In addition this stream allow you to filter the results received.
        /// </summary>
        public static ITrackedStream CreateTrackedStream(ITwitterCredentials credentials = null, TweetMode? tweetMode = null)
        {
            return GetConfiguredStream(_trackedStreamUnityFactory.Create(), credentials, tweetMode);
        }

        /// <summary>
        /// Create a stream notifying that a random tweets has been created.
        /// https://dev.twitter.com/streaming/reference/get/statuses/sample
        /// </summary>
        public static ISampleStream CreateSampleStream(ITwitterCredentials credentials = null, TweetMode? tweetMode = null)
        {
            return GetConfiguredStream(_sampleStreamUnityFactory.Create(), credentials, tweetMode);
        }

        /// <summary>
        /// Create a stream notifying the client when a tweet matching the specified criteria is created.
        /// https://dev.twitter.com/streaming/reference/post/statuses/filter
        /// </summary>
        public static IFilteredStream CreateFilteredStream(ITwitterCredentials credentials = null, TweetMode? tweetMode = null)
        {
            return GetConfiguredStream(_filteredStreamUnityFactory.Create(), credentials, tweetMode);
        }

        private static T GetConfiguredStream<T>(T stream, ITwitterCredentials credentials, TweetMode? tweetMode) where T : ITwitterStream
        {
            stream.Credentials = credentials ?? ThreadCredentialsAccessor.CurrentThreadCredentials;
            stream.TweetMode = tweetMode ?? ThreadSettingsAccessor.CurrentThreadSettings.TweetMode;

            return stream;
        }

        /// <summary>
        /// Create a stream notifying the client about everything that can happen to a user.
        /// </summary>
        public static IAccountActivityStream CreateAccountActivityStream(long userId)
        {
            var stream = _accountActivityStreamFactory.Create();

            stream.UserId = userId;

            return stream;
        }

        public static IAccountActivityStream CreateAccountActivityStream(string userId)
        {
            var longUserId = long.Parse(userId);

            return CreateAccountActivityStream(longUserId);
        }
    }
}