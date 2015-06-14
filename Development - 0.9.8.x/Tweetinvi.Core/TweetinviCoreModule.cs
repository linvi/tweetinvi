using Tweetinvi.Core.Events;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core
{
    public class TweetinviCoreModule : ITweetinviModule
    {
        public TweetinviCoreModule(ITweetinviContainer container)
        {
            _container = container;
        }

        private static ITweetinviContainer _container;
        public static ITweetinviContainer TweetinviContainer
        {
            get { return _container; }
        }

        public void Initialize()
        {
            TweetinviContainer.RegisterGeneric(typeof(IFactory<>), typeof(Factory<>));
            TweetinviContainer.RegisterType<ITaskFactory, TaskFactory>();
            TweetinviContainer.RegisterType<ISynchronousInvoker, SynchronousInvoker>();
            TweetinviContainer.RegisterType<ITweetinviSettings, TweetinviSettings>(RegistrationLifetime.InstancePerThread);
            TweetinviContainer.RegisterType<ITweetinviSettingsAccessor, TweetinviSettingsAccessor>(RegistrationLifetime.InstancePerApplication);
            TweetinviContainer.RegisterType<IThreadHelper, ThreadHelper>(RegistrationLifetime.InstancePerThread);
            TweetinviContainer.RegisterType<IAttributeHelper, AttributeHelper>(RegistrationLifetime.InstancePerApplication);
            TweetinviContainer.RegisterType<IHttpUtility, HttpUtility>(RegistrationLifetime.InstancePerApplication);
            TweetinviContainer.RegisterGeneric(typeof(IWeakEvent<>), typeof(WeakEvent<>));
            TweetinviContainer.RegisterType<ITweetinviEvents, InternalTweetinviEvents>(RegistrationLifetime.InstancePerApplication);
            TweetinviContainer.RegisterType<ISingleAggregateExceptionThrower, SingleAggregateExceptionThrower>(RegistrationLifetime.InstancePerApplication);

            InitializeParameters();
        }

        private void InitializeParameters()
        {
            // Identifiers
            _container.RegisterType<ITweetIdentifier, TweetIdentifier>();
            _container.RegisterType<IUserIdentifier, UserIdentifier>();
            _container.RegisterType<ITwitterListIdentifier, TwitterListIdentifier>();

            _container.RegisterType<IGeoCode, GeoCode>();

            // Parameters

            _container.RegisterType<ITwitterListUpdateParameters, TwitterListUpdateParameters>();
            _container.RegisterType<IGetTweetsFromListParameters, GetTweetsFromListParameters>();

            _container.RegisterType<ITweetSearchParameters, TweetSearchParameters>();
            _container.RegisterType<IUserSearchParameters, UserSearchParameters>();

            // Base
            _container.RegisterType<ICustomRequestParameters, CustomRequestParameters>();

            // Account
            _container.RegisterType<IAccountSettingsRequestParameters, AccountSettingsRequestParameters>();

            // Timeline
            _container.RegisterType<IHomeTimelineParameters, HomeTimelineParameters>();
            _container.RegisterType<IUserTimelineParameters, UserTimelineParameters>();
            _container.RegisterType<IMentionsTimelineParameters, MentionsTimelineParameters>();
            _container.RegisterType<IRetweetsOfMeTimelineRequestParameters, RetweetsOfMeTimelineRequestParameter>();

            // Message
            _container.RegisterType<IMessageGetLatestsReceivedRequestParameters, GetLatestMessagesReceivedRequestParameters>();
            _container.RegisterType<IMessageGetLatestsSentRequestParameters, GetLatestMessagesSentRequestParameters>();
        }
    }
}