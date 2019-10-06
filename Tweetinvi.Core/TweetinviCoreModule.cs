using System;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Upload;
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
            container.RegisterType<IPagedOperationsHelper, PagedOperationsHelper>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IMultiLevelCursorIteratorFactory, MultiLevelCursorIteratorFactory>(RegistrationLifetime.InstancePerApplication);

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
            container.RegisterType<ITwitterListUpdateParameters, TwitterListUpdateParameters>();
            container.RegisterType<IGetTweetsFromListParameters, GetTweetsFromListParameters>();

            // Account
            container.RegisterType<IUpdateProfileBannerParameters, UpdateProfileBannerParameters>();

            // Search
            container.RegisterType<ISearchTweetsParameters, SearchTweetsParameters>();
            container.RegisterType<ISearchUsersParameters, SearchUsersParameters>();

            // Tweet
            container.RegisterType<IPublishTweetParameters, PublishTweetParameters>();

            // Timeline
            container.RegisterType<IHomeTimelineParameters, HomeTimelineParameters>();
            container.RegisterType<IUserTimelineParameters, UserTimelineParameters>();
            container.RegisterType<IMentionsTimelineParameters, MentionsTimelineParameters>();
            container.RegisterType<IRetweetsOfMeTimelineParameters, RetweetsOfMeTimelineParameter>();

            // Message
            container.RegisterType<IGetMessagesParameters, GetMessagesParameters>();
            container.RegisterType<IPublishMessageParameters, PublishMessageParameters>();

            // Upload
            container.RegisterType<IChunkUploadInitParameters, ChunkUploadInitParameters>();
            container.RegisterType<IChunkUploadAppendParameters, ChunkUploadAppendParameters>();
        }

        private void InitializeParametersValidators(ITweetinviContainer container)
        {
            container.RegisterType<IInternalParametersValidator, ParametersValidator>();
            
            container.RegisterType<IInternalAccountClientParametersValidator, AccountClientParametersValidator>();
            container.RegisterType<IAccountClientRequiredParametersValidator, AccountClientRequiredParametersValidator>();
            
            container.RegisterType<IInternalAccountSettingsClientParametersValidator, AccountSettingsClientParametersValidator>();
            container.RegisterType<IAccountSettingsClientRequiredParametersValidator, AccountSettingsClientRequiredParametersValidator>();

            container.RegisterType<IInternalUploadClientParametersValidator, UploadClientParametersValidator>();
            container.RegisterType<IUploadClientRequiredParametersValidator, UploadClientRequiredParametersValidator>();

            container.RegisterType<IInternalTweetsClientParametersValidator, TweetsClientParametersValidator>();
            container.RegisterType<ITweetsClientRequiredParametersValidator, TweetsClientRequiredParametersValidator>();
            
            container.RegisterType<IInternalUsersClientParametersValidator, UsersClientParametersValidator>();
            container.RegisterType<IUsersClientRequiredParametersValidator, UsersClientRequiredParametersValidator>();
        }
    }
}