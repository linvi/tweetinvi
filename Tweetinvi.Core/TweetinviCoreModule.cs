using Tweetinvi.Core.Events;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;
using Tweetinvi.Core.Web;

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
            _container.RegisterGeneric(typeof(IFactory<>), typeof(Factory<>));
            _container.RegisterType<ITaskFactory, TaskFactory>();
            _container.RegisterType<ISynchronousInvoker, SynchronousInvoker>();
            _container.RegisterType<ITweetinviSettings, TweetinviSettings>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITweetinviSettingsAccessor, TweetinviSettingsAccessor>(RegistrationLifetime.InstancePerApplication);
            _container.RegisterType<IThreadHelper, ThreadHelper>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IAttributeHelper, AttributeHelper>(RegistrationLifetime.InstancePerApplication);
            _container.RegisterType<IHttpUtility, HttpUtility>(RegistrationLifetime.InstancePerApplication);
            _container.RegisterGeneric(typeof(IWeakEvent<>), typeof(WeakEvent<>));
            _container.RegisterType<ITweetinviEvents, InternalTweetinviEvents>(RegistrationLifetime.InstancePerApplication);
            _container.RegisterType<ISingleAggregateExceptionThrower, SingleAggregateExceptionThrower>(RegistrationLifetime.InstancePerApplication);
            _container.RegisterType<ITwitterExceptionFactory, TwitterExceptionFactory>();
            _container.RegisterType<ITwitterException, TwitterException>();

            InitializeParameters();
        }

        private void InitializeParameters()
        {
            // Base
            _container.RegisterType<ICustomRequestParameters, CustomRequestParameters>();
            
            // Identifiers
            _container.RegisterType<ITweetIdentifier, TweetIdentifier>();
            _container.RegisterType<IUserIdentifier, UserIdentifier>();
            _container.RegisterType<ITwitterListIdentifier, TwitterListIdentifier>();

            _container.RegisterType<IGeoCode, GeoCode>();

            // Parameters
            _container.RegisterType<ITwitterListUpdateParameters, TwitterListUpdateParameters>();
            _container.RegisterType<IGetTweetsFromListParameters, GetTweetsFromListParameters>();

            // Account
            _container.RegisterType<IAccountUpdateProfileBannerParameters, AccountUpdateProfileBannerParameters>();

            // Search
            _container.RegisterType<ITweetSearchParameters, TweetSearchParameters>();
            _container.RegisterType<IUserSearchParameters, UserSearchParameters>();

            // Tweet
            _container.RegisterType<IPublishTweetParameters, PublishTweetParameters>();
            _container.RegisterType<IPublishTweetOptionalParameters, PublishTweetOptionalParameters>();
            _container.RegisterType<IGetUserFavoritesParameters, GetUserFavoritesParameters>();

            // Account
            _container.RegisterType<IAccountSettingsRequestParameters, AccountSettingsRequestParameters>();

            // Timeline
            _container.RegisterType<IHomeTimelineParameters, HomeTimelineParameters>();
            _container.RegisterType<IUserTimelineParameters, UserTimelineParameters>();
            _container.RegisterType<IMentionsTimelineParameters, MentionsTimelineParameters>();
            _container.RegisterType<IRetweetsOfMeTimelineParameters, RetweetsOfMeTimelineParameter>();

            // Message
            _container.RegisterType<IMessagesReceivedParameters, MessagesReceivedParameters>();
            _container.RegisterType<IMessagesSentParameters, MessagesSentParameters>();
            _container.RegisterType<IPublishMessageParameters, PublishMessageParameters>();

            // Upload
            _container.RegisterType<IChunkUploadInitParameters, ChunkUploadInitParameters>();
            _container.RegisterType<IChunkUploadAppendParameters, ChunkUploadAppendParameters>();
        }
    }
}