using System;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Json;
using Tweetinvi.Core.Upload;
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
        public static ITweetinviContainer TweetinviContainer => _container;

        public void Initialize(ITweetinviContainer container)
        {
            if (container != _container)
            {
                throw new InvalidOperationException("This container can only be initialized with the main container");
            }

            container.RegisterGeneric(typeof(IFactory<>), typeof(Factory<>));
            container.RegisterType<ITaskFactory, TaskFactory>();
            container.RegisterType<ISynchronousInvoker, SynchronousInvoker>();
            container.RegisterType<ITaskDelayer, TaskDelayer>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITweetinviSettingsAccessor, TweetinviSettingsAccessor>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IAttributeHelper, AttributeHelper>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IHttpUtility, HttpUtility>(RegistrationLifetime.InstancePerApplication);
            container.RegisterGeneric(typeof(IWeakEvent<>), typeof(WeakEvent<>));
            container.RegisterInstance(typeof(ITweetinviEvents), new TweetinviEvents());
            container.RegisterType<ITwitterClientEvents, TwitterClientEvents>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ISingleAggregateExceptionThrower, SingleAggregateExceptionThrower>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterExceptionFactory, TwitterExceptionFactory>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterException, TwitterException>();
            container.RegisterType<IPagedOperationsHelper, PagedOperationsHelper>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<JsonContentFactory, JsonContentFactory>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IMultiLevelCursorIteratorFactory, MultiLevelCursorIteratorFactory>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IPageCursorIteratorFactories, PageCursorIteratorFactories>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ITweetinviJsonConverter, TweetinviJsonConverter>(RegistrationLifetime.InstancePerApplication);

            InitializeParameters(container);
            InitializeParametersValidators(container);
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
            container.RegisterType<IGetTweetsFromListParameters, GetTweetsFromListParameters>();

            // Account
            container.RegisterType<IUpdateProfileBannerParameters, UpdateProfileBannerParameters>();

            // Search
            container.RegisterType<ISearchTweetsParameters, SearchTweetsParameters>();
            container.RegisterType<ISearchUsersParameters, SearchUsersParameters>();

            // Tweet
            container.RegisterType<IPublishTweetParameters, PublishTweetParameters>();

            // Timeline
            container.RegisterType<IGetHomeTimelineParameters, GetHomeTimelineParameters>();
            container.RegisterType<IGetMentionsTimelineParameters, GetMentionsTimelineParameters>();
            container.RegisterType<IGetRetweetsOfMeTimelineParameters, GetRetweetsOfMeTimelineParameters>();

            // Message
            container.RegisterType<IPublishMessageParameters, PublishMessageParameters>();

            // Upload
            container.RegisterType<IChunkUploadInitParameters, ChunkUploadInitParameters>();
            container.RegisterType<IChunkUploadAppendParameters, ChunkUploadAppendParameters>();
        }

        private static void InitializeParametersValidators(ITweetinviContainer container)
        {
            container.RegisterType<IParametersValidator, ParametersValidator>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IAccountActivityClientParametersValidator, AccountActivityClientParametersValidator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IAccountActivityClientRequiredParametersValidator, AccountActivityClientRequiredParametersValidator>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IAccountSettingsClientParametersValidator, AccountSettingsClientParametersValidator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IAccountSettingsClientRequiredParametersValidator, AccountSettingsClientRequiredParametersValidator>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IAuthClientParametersValidator, AuthClientParametersValidator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IAuthClientRequiredParametersValidator, AuthClientRequiredParametersValidator>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IHelpClientParametersValidator, HelpClientParametersValidator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IHelpClientRequiredParametersValidator, HelpClientRequiredParametersValidator>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ITwitterListsClientParametersValidator, TwitterListsClientParametersValidator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterListsClientRequiredParametersValidator, TwitterListsClientRequiredParametersValidator>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IMessagesClientParametersValidator, MessagesClientParametersValidator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IMessagesClientRequiredParametersValidator, MessagesClientRequiredParametersValidator>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ITimelineClientParametersValidator, TimelineClientParametersValidator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITimelineClientRequiredParametersValidator, TimelineClientRequiredParametersValidator>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ITweetsClientParametersValidator, TweetsClientParametersValidator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITweetsClientRequiredParametersValidator, TweetsClientRequiredParametersValidator>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IUploadClientParametersValidator, UploadClientParametersValidator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IUploadClientRequiredParametersValidator, UploadClientRequiredParametersValidator>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IUsersClientParametersValidator, UsersClientParametersValidator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IUsersClientRequiredParametersValidator, UsersClientRequiredParametersValidator>(RegistrationLifetime.InstancePerApplication);
        }
    }
}