using Tweetinvi.Controllers.Account;
using Tweetinvi.Controllers.AccountSettings;
using Tweetinvi.Controllers.Auth;
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
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Core.Upload;

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
        }

        private void InitializeControllers(ITweetinviContainer container)
        {
            container.RegisterType<IAccountController, AccountController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IAuthController, AuthController>();
            container.RegisterType<IAccountSettingsController, AccountSettingsController>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IGeoController, GeoController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IHelpController, HelpController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IMessageController, MessageController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ISavedSearchController, SavedSearchController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITimelineController, TimelineController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITrendsController, TrendsController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITweetController, TweetController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IUserController, UserController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterListController, TwitterListController>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ISearchController, SearchController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IAccountActivityController, AccountActivityController>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IChunkedUploader, ChunkedUploader>();
        }

        private void InitializeJsonControllers(ITweetinviContainer container)
        {
            container.RegisterType<IGeoJsonController, GeoJsonController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IMessageJsonController, MessageJsonController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ISavedSearchJsonController, SavedSearchJsonController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITrendsJsonController, TrendsJsonController>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ISearchJsonController, SearchJsonController>(RegistrationLifetime.InstancePerApplication);
        }

        private void InitializeQueryExecutors(ITweetinviContainer container)
        {
            container.RegisterType<IAccountSettingsQueryExecutor, AccountSettingsQueryExecutor>();
            container.RegisterType<IAuthQueryExecutor, AuthQueryExecutor>();

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
            container.RegisterType<IAccountSettingsQueryGenerator, AccountSettingsQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IAuthQueryGenerator, AuthQueryGenerator>(RegistrationLifetime.InstancePerApplication);

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
            container.RegisterType<IAccountActivityQueryGenerator, AccountActivityQueryGenerator>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IQueryParameterGenerator, QueryParameterGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterListQueryParameterGenerator, TwitterListQueryParameterGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IUserQueryParameterGenerator, UserQueryParameterGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ISearchQueryParameterGenerator, SearchQueryParameterGenerator>(RegistrationLifetime.InstancePerApplication);

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
    }
}