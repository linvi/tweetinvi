using System;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

// ReSharper disable once CheckNamespace
namespace Tweetinvi.Core.Injectinvi
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

        public void Initialize(ITweetinviContainer container)
        {
            if (container != _container)
            {
                throw new InvalidOperationException("This container can only be initialized with the main container");
            }

            container.RegisterGeneric(typeof(IFactory<>), typeof(Factory<>));
            container.RegisterType<ITaskFactory, TaskFactory>();
            container.RegisterType<ISynchronousInvoker, SynchronousInvoker>();
            container.RegisterType<ITweetinviSettings, TweetinviSettings>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ITweetinviSettingsAccessor, TweetinviSettingsAccessor>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IThreadHelper, ThreadHelper>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<IAttributeHelper, AttributeHelper>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IHttpUtility, HttpUtility>(RegistrationLifetime.InstancePerApplication);
            container.RegisterGeneric(typeof(IWeakEvent<>), typeof(WeakEvent<>));
            container.RegisterType<ITweetinviEvents, InternalTweetinviEvents>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ISingleAggregateExceptionThrower, SingleAggregateExceptionThrower>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterExceptionFactory, TwitterExceptionFactory>();
            container.RegisterType<ITwitterException, TwitterException>();

            InitializeParameters(container);
        }

        private void InitializeParameters(ITweetinviContainer container)
        {
            // Base
            container.RegisterType<ICustomRequestParameters, CustomRequestParameters>();
            
            // Identifiers
            container.RegisterType<ITweetIdentifier, TweetIdentifier>();
            container.RegisterType<IUserIdentifier, UserIdentifier>();
            container.RegisterType<ITwitterListIdentifier, TwitterListIdentifier>();

            container.RegisterType<IGeoCode, GeoCode>();

            // Parameters
            container.RegisterType<ITwitterListUpdateParameters, TwitterListUpdateParameters>();
            container.RegisterType<IGetTweetsFromListParameters, GetTweetsFromListParameters>();

            // Account
            container.RegisterType<IAccountUpdateProfileBannerParameters, AccountUpdateProfileBannerParameters>();

            // Search
            container.RegisterType<ISearchTweetsParameters, SearchTweetsParameters>();
            container.RegisterType<ISearchUsersParameters, SearchUsersParameters>();

            // Tweet
            container.RegisterType<IPublishTweetParameters, PublishTweetParameters>();
            container.RegisterType<IGetUserFavoritesParameters, GetUserFavoritesParameters>();

            // Account
            container.RegisterType<IAccountSettingsRequestParameters, AccountSettingsRequestParameters>();

            // Timeline
            container.RegisterType<IHomeTimelineParameters, HomeTimelineParameters>();
            container.RegisterType<IUserTimelineParameters, UserTimelineParameters>();
            container.RegisterType<IMentionsTimelineParameters, MentionsTimelineParameters>();
            container.RegisterType<IRetweetsOfMeTimelineParameters, RetweetsOfMeTimelineParameter>();

            // Message
            _container.RegisterType<IGetMessagesParameters, GetMessagesParameters>();
            _container.RegisterType<IPublishMessageParameters, PublishMessageParameters>();

            // Upload
            container.RegisterType<IChunkUploadInitParameters, ChunkUploadInitParameters>();
            container.RegisterType<IChunkUploadAppendParameters, ChunkUploadAppendParameters>();
        }
    }
}