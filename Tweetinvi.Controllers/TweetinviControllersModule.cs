using Tweetinvi.Controllers.Account;
using Tweetinvi.Controllers.Friendship;
using Tweetinvi.Controllers.Geo;
using Tweetinvi.Controllers.Help;
using Tweetinvi.Controllers.Messages;
using Tweetinvi.Controllers.SavedSearch;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Controllers.Timeline;
using Tweetinvi.Controllers.Trends;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Controllers.TwitterLists;
using Tweetinvi.Controllers.Upload;
using Tweetinvi.Controllers.User;
using Tweetinvi.Controllers.Webhooks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers
{
    public class TweetinviControllersModule : ITweetinviModule
    {
        public void Initialize(ITweetinviContainer container)
        {
            InitializeControllers(container);
            InitializeJsonControllers(container);
            InitializeQueryExecutors(container);
            InitializeQueryGenerators(container);
            InitializeQueryValidators(container);
            InitializeHelpers(container);
            InitializeParameters(container);
        }

        private void InitializeControllers(ITweetinviContainer container)
        {
            container.RegisterType<IAccountController, AccountController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<IFriendshipController, FriendshipController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<IGeoController, GeoController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<IHelpController, HelpController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<IMessageController, MessageController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ISavedSearchController, SavedSearchController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ITimelineController, TimelineController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ITrendsController, TrendsController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ITweetController, TweetController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<IUserController, UserController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ITwitterListController, TwitterListController>(RegistrationLifetime.InstancePerThread);

            container.RegisterType<ISearchController, SearchController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<IWebhookController, WebhookController>(RegistrationLifetime.InstancePerThread);

            container.RegisterType<IChunkedUploader, ChunkedUploader>();
        }

        private void InitializeJsonControllers(ITweetinviContainer container)
        {
            container.RegisterType<IAccountJsonController, AccountJsonController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<IFriendshipJsonController, FriendshipJsonController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<IGeoJsonController, GeoJsonController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<IHelpJsonController, HelpJsonController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<IMessageJsonController, MessageJsonController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ISavedSearchJsonController, SavedSearchJsonController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ITimelineJsonController, TimelineJsonController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ITrendsJsonController, TrendsJsonController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ITweetJsonController, TweetJsonController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<IUserJsonController, UserJsonController>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ITwitterListJsonController, TwitterListJsonController>(RegistrationLifetime.InstancePerThread);

            container.RegisterType<ISearchJsonController, SearchJsonController>(RegistrationLifetime.InstancePerThread);
        }

        private void InitializeQueryExecutors(ITweetinviContainer container)
        {
            container.RegisterType<IAccountQueryExecutor, AccountQueryExecutor>();
            container.RegisterType<IFriendshipQueryExecutor, FriendshipQueryExecutor>();
            container.RegisterType<IGeoQueryExecutor, GeoQueryExecutor>();
            container.RegisterType<IHelpQueryExecutor, HelpQueryExecutor>();
            container.RegisterType<IMessageQueryExecutor, MessageQueryExecutor>();
            container.RegisterType<ISavedSearchQueryExecutor, SavedSearchQueryExecutor>();
            container.RegisterType<ITimelineQueryExecutor, TimelineQueryExecutor>();
            container.RegisterType<ITrendsQueryExecutor, TrendsQueryExecutor>();
            container.RegisterType<ITweetQueryExecutor, TweetQueryExecutor>();
            container.RegisterType<IUserQueryExecutor, UserQueryExecutor>();
            container.RegisterType<ITwitterListQueryExecutor, TwitterListQueryExecutor>();

            container.RegisterType<ISearchQueryExecutor, SearchQueryExecutor>();
            container.RegisterType<IUploadQueryExecutor, UploadQueryExecutor>();
            container.RegisterType<IUploadMediaStatusQueryExecutor, UploadMediaStatusQueryExecutor>();
        }

        private void InitializeQueryGenerators(ITweetinviContainer container)
        {
            container.RegisterType<IAccountQueryGenerator, AccountQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IFriendshipQueryGenerator, FriendshipQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IGeoQueryGenerator, GeoQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IHelpQueryGenerator, HelpQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IMessageQueryGenerator, MessageQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ISavedSearchQueryGenerator, SavedSearchQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITimelineQueryGenerator, TimelineQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITrendsQueryGenerator, TrendsQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITweetQueryGenerator, TweetQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IUserQueryGenerator, UserQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ISearchQueryGenerator, SearchQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterListQueryGenerator, TwitterListQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            
            container.RegisterType<IQueryParameterGenerator, QueryParameterGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterListQueryParameterGenerator, TwittertListQueryParameterGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IUserQueryParameterGenerator, UserQueryParameterGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ISearchQueryParameterGenerator, SearchQueryParameterGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITimelineQueryParameterGenerator, TimelineQueryParameterGenerator>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IUploadQueryGenerator, UploadQueryGenerator>(RegistrationLifetime.InstancePerApplication);
        }

        private void InitializeQueryValidators(ITweetinviContainer container)
        {
            container.RegisterType<IMessageQueryValidator, MessageQueryValidator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITweetQueryValidator, TweetQueryValidator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IUserQueryValidator, UserQueryValidator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ISearchQueryValidator, SearchQueryValidator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterListQueryValidator, TwitterListQueryValidator>(RegistrationLifetime.InstancePerApplication);
        }

        private void InitializeHelpers(ITweetinviContainer container)
        {
            container.RegisterType<ITweetHelper, TweetHelper>();
            container.RegisterType<ISearchQueryHelper, SearchQueryHelper>();
            container.RegisterType<IUploadHelper, UploadHelper>(RegistrationLifetime.InstancePerApplication);
        }

        private void InitializeParameters(ITweetinviContainer container)
        {
            container.RegisterType<IFriendshipAuthorizations, FriendshipAuthorizations>();
        }
    }
}