using Tweetinvi.Client;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi
{
    public class TweetinviModule : ITweetinviModule
    {
        public void Initialize(ITweetinviContainer container)
        {
            // Register a singleton of the container, do not use InstancePerApplication
            container.RegisterInstance(typeof(ITweetinviContainer), container);

            container.RegisterType<IAuthClient, AuthClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IAuthRequester, AuthRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IAccountSettingsClient, AccountSettingsClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IAccountSettingsRequester, AccountSettingsRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IExecuteClient, ExecuteClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IExecuteRequester, ExecuteRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IRateLimitsClient, RateLimitsClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IHelpClient, HelpClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IHelpRequester, HelpRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IListsClient, ListsClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterListsRequester, TwitterListsRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IMessagesClient, MessagesClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IMessageRequester, MessageRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ISearchClient, SearchClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ISearchRequester, SearchRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IStreamsClient, StreamsClient>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ITimelinesClient, TimelinesClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITimelinesRequester, TimelinesRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ITrendsClient, TrendsClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITrendsRequester, TrendsRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ITweetsClient, TweetsClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITweetsRequester, TweetsRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IUploadClient, UploadClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IUploadRequester, UploadRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IUsersClient, UsersClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IUsersRequester, UsersRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IAccountActivityClient, AccountActivityClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IAccountActivityRequester, AccountActivityRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IRawExecutors, RawExecutors>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterClientFactories, TwitterClientFactories>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IJsonClient, JsonClient>(RegistrationLifetime.InstancePerApplication);
        }
    }
}