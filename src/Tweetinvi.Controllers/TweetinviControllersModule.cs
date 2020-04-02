using Tweetinvi.Controllers.AccountSettings;
using Tweetinvi.Controllers.Auth;
using Tweetinvi.Controllers.Help;
using Tweetinvi.Controllers.Messages;
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
using Tweetinvi.Core.Upload;

namespace Tweetinvi.Controllers
{
    public class TweetinviControllersModule : ITweetinviModule
    {
        public void Initialize(ITweetinviContainer container)
        {
            InitializeControllers(container);
            InitializeQueryExecutors(container);
            InitializeQueryGenerators(container);
            InitializeHelpers(container);
        }

        private void InitializeControllers(ITweetinviContainer container)
        {
            container.RegisterType<IAuthController, AuthController>();
            container.RegisterType<IAccountSettingsController, AccountSettingsController>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IHelpController, HelpController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IMessageController, MessageController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITimelineController, TimelineController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITrendsController, TrendsController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITweetController, TweetController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IUserController, UserController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterListController, TwitterListController>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ISearchController, SearchController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IAccountActivityController, AccountActivityController>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IChunkedUploader, ChunkedUploader>();
        }

        private void InitializeQueryExecutors(ITweetinviContainer container)
        {
            container.RegisterType<IAccountSettingsQueryExecutor, AccountSettingsQueryExecutor>();
            container.RegisterType<IAuthQueryExecutor, AuthQueryExecutor>();

            container.RegisterType<IHelpQueryExecutor, HelpQueryExecutor>();
            container.RegisterType<IMessageQueryExecutor, MessageQueryExecutor>();
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
            container.RegisterType<IAccountSettingsQueryGenerator, AccountSettingsQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IAuthQueryGenerator, AuthQueryGenerator>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IHelpQueryGenerator, HelpQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IMessageQueryGenerator, MessageQueryGenerator>(RegistrationLifetime.InstancePerApplication);
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

        private void InitializeHelpers(ITweetinviContainer container)
        {
            container.RegisterType<ITweetHelper, TweetHelper>();
            container.RegisterType<IUploadHelper, UploadHelper>(RegistrationLifetime.InstancePerApplication);
        }
    }
}