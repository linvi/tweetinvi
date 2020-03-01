using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Streaming;
using Tweetinvi.Streaming.Events;
using Tweetinvi.Streaming.Webhooks;
using Tweetinvi.Streams.Helpers;
using Tweetinvi.Streams.Model;
using Tweetinvi.Streams.Webhooks;

namespace Tweetinvi.Streams
{
    public class StreaminviModule : ITweetinviModule
    {
        public void Initialize(ITweetinviContainer container)
        {
            container.RegisterType<ITweetStream, TweetStream>();
            container.RegisterType<ISampleStream, SampleStream>();
            container.RegisterType<ITrackedStream, TrackedStream>();
            container.RegisterType<IFilteredStream, FilteredStream>();

            container.RegisterType<AccountActivityStream, AccountActivityStream>();
            container.RegisterType<IAccountActivityStream, AccountActivityStream>();

            container.RegisterType<IFilterStreamTweetMatcher, FilterStreamTweetMatcher>();
            container.RegisterType<IFilterStreamTweetMatcherFactory, FilterStreamTweetMatcherFactory>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IWarningMessage, WarningMessage>();
            container.RegisterType<IWarningMessageTooManyFollowers, WarningMessageTooManyFollowers>();
            container.RegisterType<IWarningMessageFallingBehind, WarningMessageFallingBehind>();

            container.RegisterType<IStreamTask, StreamTask>();
            container.RegisterType<IStreamResultGenerator, StreamResultGenerator>();

            container.RegisterGeneric(typeof(IStreamTrackManager<>), typeof(StreamTrackManager<>));

            container.RegisterType<IWebhookDispatcher, WebhookDispatcher>();
            container.RegisterType<IStreamTaskFactory, StreamTaskFactory>(RegistrationLifetime.InstancePerApplication);
        }
    }
}