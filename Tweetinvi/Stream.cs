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

        public static ITweetStream CreateTweetStream()
        {
            return _tweetStreamUnityFactory.Create();
        }

        public static ITrackedStream CreateTrackedStream()
        {
            return _trackedStreamUnityFactory.Create();
        }

        public static ISampleStream CreateSampleStream()
        {
            return _sampleStreamUnityFactory.Create();
        }

        public static IFilteredStream CreateFilteredStream()
        {
            return _filteredStreamUnityFactory.Create();
        }

        public static IUserStream CreateUserStream()
        {
            return _userStreamFactory.Create();
        }
    }
}