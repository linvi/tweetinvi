using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Streaming;
using Tweetinvi.Streaming.Events;
using Tweetinvi.Streams.Helpers;
using Tweetinvi.Streams.Model;

namespace Tweetinvi.Streams
{
    public class StreaminviModule : ITweetinviModule
    {
        private readonly ITweetinviContainer _container;

        public StreaminviModule(ITweetinviContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IUserStream, UserStream>();
            _container.RegisterType<ITweetStream, TweetStream>();
            _container.RegisterType<ISampleStream, SampleStream>();
            _container.RegisterType<ITrackedStream, TrackedStream>();
            _container.RegisterType<IFilteredStream, FilteredStream>();

            _container.RegisterType<IWarningMessage, WarningMessage>();
            _container.RegisterType<IWarningMessageTooManyFollowers, WarningMessageTooManyFollowers>();
            _container.RegisterType<IWarningMessageFallingBehind, WarningMessageFallingBehind>();

            _container.RegisterType<IStreamTask, StreamTask>();
            _container.RegisterType<IStreamResultGenerator, StreamResultGenerator>();

            _container.RegisterGeneric(typeof(IStreamTrackManager<>), typeof(StreamTrackManager<>));
        }
    }
}