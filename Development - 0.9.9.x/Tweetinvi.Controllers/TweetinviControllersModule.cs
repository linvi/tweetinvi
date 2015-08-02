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
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Controllers.Transactions;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Tweetinvi.Controllers
{
    public class TweetinviControllersModule : ITweetinviModule
    {
        private readonly ITweetinviContainer _container;

        public TweetinviControllersModule(ITweetinviContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            InitializeControllers();
            InitializeJsonControllers();
            InitializeQueryExecutors();
            InitializeQueryGenerators();
            InitializeQueryValidators();
            InitializeHelpers();
            InitializeParameters();
        }

        private void InitializeControllers()
        {
            _container.RegisterType<IAccountController, AccountController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IFriendshipController, FriendshipController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IGeoController, GeoController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IHelpController, HelpController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IMessageController, MessageController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ISavedSearchController, SavedSearchController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITimelineController, TimelineController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITrendsController, TrendsController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITweetController, TweetController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IUserController, UserController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITwitterListController, TwitterListController>(RegistrationLifetime.InstancePerThread);

            _container.RegisterType<ISearchController, SearchController>(RegistrationLifetime.InstancePerThread);


            _container.RegisterType<IChunkedUploader, ChunkedUploader>();
        }

        private void InitializeJsonControllers()
        {
            _container.RegisterType<IAccountJsonController, AccountJsonController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IFriendshipJsonController, FriendshipJsonController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IGeoJsonController, GeoJsonController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IHelpJsonController, HelpJsonController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IMessageJsonController, MessageJsonController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ISavedSearchJsonController, SavedSearchJsonController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITimelineJsonController, TimelineJsonController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITrendsJsonController, TrendsJsonController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITweetJsonController, TweetJsonController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IUserJsonController, UserJsonController>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITwitterListJsonController, TwitterListJsonController>(RegistrationLifetime.InstancePerThread);

            _container.RegisterType<ISearchJsonController, SearchJsonController>(RegistrationLifetime.InstancePerThread);
        }

        private void InitializeQueryExecutors()
        {
            _container.RegisterType<IAccountQueryExecutor, AccountQueryExecutor>();
            _container.RegisterType<IFriendshipQueryExecutor, FriendshipQueryExecutor>();
            _container.RegisterType<IGeoQueryExecutor, GeoQueryExecutor>();
            _container.RegisterType<IHelpQueryExecutor, HelpQueryExecutor>();
            _container.RegisterType<IMessageQueryExecutor, MessageQueryExecutor>();
            _container.RegisterType<ISavedSearchQueryExecutor, SavedSearchQueryExecutor>();
            _container.RegisterType<ITimelineQueryExecutor, TimelineQueryExecutor>();
            _container.RegisterType<ITrendsQueryExecutor, TrendsQueryExecutor>();
            _container.RegisterType<ITweetQueryExecutor, TweetQueryExecutor>();
            _container.RegisterType<IUserQueryExecutor, UserQueryExecutor>();
            _container.RegisterType<ITwitterListQueryExecutor, TwitterListQueryExecutor>();

            _container.RegisterType<ISearchQueryExecutor, SearchQueryExecutor>();
            _container.RegisterType<IUploadQueryExecutor, UploadQueryExecutor>();
        }

        private void InitializeQueryGenerators()
        {
            _container.RegisterType<IAccountQueryGenerator, AccountQueryGenerator>();
            _container.RegisterType<IFriendshipQueryGenerator, FriendshipQueryGenerator>();
            _container.RegisterType<IGeoQueryGenerator, GeoQueryGenerator>();
            _container.RegisterType<IHelpQueryGenerator, HelpQueryGenerator>();
            _container.RegisterType<IMessageQueryGenerator, MessageQueryGenerator>();
            _container.RegisterType<ISavedSearchQueryGenerator, SavedSearchQueryGenerator>();
            _container.RegisterType<ITimelineQueryGenerator, TimelineQueryGenerator>();
            _container.RegisterType<ITrendsQueryGenerator, TrendsQueryGenerator>();
            _container.RegisterType<ITweetQueryGenerator, TweetQueryGenerator>();
            _container.RegisterType<IUserQueryGenerator, UserQueryGenerator>();
            _container.RegisterType<ISearchQueryGenerator, SearchQueryGenerator>();
            _container.RegisterType<ITwitterListQueryGenerator, TwitterListQueryGenerator>();
            
            _container.RegisterType<IQueryParameterGenerator, QueryParameterGenerator>();
            _container.RegisterType<ITwitterListQueryParameterGenerator, TwittertListQueryParameterGenerator>();
            _container.RegisterType<IUserQueryParameterGenerator, UserQueryParameterGenerator>();
            _container.RegisterType<ISearchQueryParameterGenerator, SearchQueryParameterGenerator>();
            _container.RegisterType<ITimelineQueryParameterGenerator, TimelineQueryParameterGenerator>();

            _container.RegisterType<IUploadQueryGenerator, UploadQueryGenerator>(RegistrationLifetime.InstancePerApplication);
        }

        private void InitializeQueryValidators()
        {
            _container.RegisterType<IMessageQueryValidator, MessageQueryValidator>();
            _container.RegisterType<ITweetQueryValidator, TweetQueryValidator>();
            _container.RegisterType<IUserQueryValidator, UserQueryValidator>();
            _container.RegisterType<ISearchQueryValidator, SearchQueryValidator>();
            _container.RegisterType<ITwitterListQueryValidator, TwitterListQueryValidator>();
        }

        private void InitializeHelpers()
        {
            _container.RegisterType<ITweetHelper, TweetHelper>();
            _container.RegisterType<ISearchQueryHelper, SearchQueryHelper>();
        }

        private void InitializeParameters()
        {
            _container.RegisterType<IFriendshipAuthorizations, FriendshipAuthorizations>();
        }
    }
}