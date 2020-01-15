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

            container.RegisterType<IAccountClient, AccountClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IInternalAccountRequester, AccountRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IAuthClient, AuthClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IInternalAuthRequester, AuthRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IAccountSettingsClient, AccountSettingsClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IInternalAccountSettingsRequester, AccountSettingsRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IExecuteClient, ExecuteClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IInternalExecuteRequester, ExecuteRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IRateLimitsClient, RateLimitsClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IHelpClient, HelpClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IInternalHelpRequester, HelpRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IStreamClient, StreamClient>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ITimelineClient, TimelineClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IInternalTimelineRequester, TimelineRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ITweetsClient, TweetsClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IInternalTweetsRequester, TweetsRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IUploadClient, UploadClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IInternalUploadRequester, UploadRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IUsersClient, UsersClient>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IInternalUsersRequester, UsersRequester>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IInternalRequestExecutor, RequestExecutor>();
            container.RegisterType<ITwitterClientFactories, TwitterClientFactories>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterClientJson, TwitterClientJson>(RegistrationLifetime.InstancePerApplication);
        }
    }
}