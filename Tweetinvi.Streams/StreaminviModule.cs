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
        public void Initialize(ITweetinviContainer container)
        {
            container.RegisterType<IUserStream, UserStream>();
            container.RegisterType<ITweetStream, TweetStream>();
            container.RegisterType<ISampleStream, SampleStream>();
            container.RegisterType<ITrackedStream, TrackedStream>();
            container.RegisterType<IFilteredStream, FilteredStream>();

            container.RegisterType<IFilterStreamTweetMatcher, FilterStreamTweetMatcher>();
            container.RegisterType<IFilterStreamTweetMatcherFactory, FilterStreamTweetMatcherFactory>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IWarningMessage, WarningMessage>();
            container.RegisterType<IWarningMessageTooManyFollowers, WarningMessageTooManyFollowers>();
            container.RegisterType<IWarningMessageFallingBehind, WarningMessageFallingBehind>();

            container.RegisterType<IStreamTask, StreamTask>();
            container.RegisterType<IStreamResultGenerator, StreamResultGenerator>();

            container.RegisterGeneric(typeof(IStreamTrackManager<>), typeof(StreamTrackManager<>));
        }
    }
}