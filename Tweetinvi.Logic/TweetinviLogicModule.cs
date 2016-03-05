using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Exceptions;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Logic.Exceptions;
using Tweetinvi.Logic.Helpers;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Logic.Model;
using Tweetinvi.Logic.QueryParameters;
using Tweetinvi.Logic.TwitterEntities;
using Tweetinvi.Logic.Wrapper;

namespace Tweetinvi.Logic
{
    public class TweetinviLogicModule : ITweetinviModule
    {
        private readonly ITweetinviContainer _container;

        public TweetinviLogicModule(ITweetinviContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            InitializeTwitterModels();
            InitializeTweetinviModels();

            InitializeDTOs();
            InitializeHelpers();
            InitializeWrappers();
            InitializeQueryParameters();
            InitializeExceptionHandler();
            InitializeSerialization();
        }

        // Initialize Models that are not objects coming from Twitter
        private void InitializeTweetinviModels()
        {
            _container.RegisterType<IMedia, Media>();
            _container.RegisterType<IEditableMedia, Media>();
            _container.RegisterType<ITwitterQuery, TwitterQuery>();
            _container.RegisterType<ISearchQueryResult, SearchQueryResult>();
        }

        // Initialize Models that are Twitter objects
        private void InitializeTwitterModels()
        {
            _container.RegisterType<ITweet, Tweet>();
            _container.RegisterType<ITweetWithSearchMetadata, TweetWithSearchMetadata>();
            _container.RegisterType<IOEmbedTweet, OEmbedTweet>();

            _container.RegisterType<IUser, User>();
            _container.RegisterType<IAuthenticatedUser, AuthenticatedUser>();

            _container.RegisterType<ITwitterList, TwitterList>();

            _container.RegisterType<ICoordinates, CoordinatesDTO>();
            _container.RegisterType<ILocation, Location>();

            _container.RegisterType<IAccountSettings, AccountSettings>();
            _container.RegisterType<IMessage, Message>();
            _container.RegisterType<IMention, Mention>();
            _container.RegisterType<IRelationshipDetails, RelationshipDetails>();
            _container.RegisterType<IRelationshipState, RelationshipState>();
            _container.RegisterType<ISavedSearch, SavedSearch>();
        }

        private void InitializeDTOs()
        {
            _container.RegisterType<ITweetDTO, TweetDTO>();
            _container.RegisterType<ITwitterListDTO, TwitterListDTO>();
            _container.RegisterType<IUserDTO, UserDTO>();
            _container.RegisterType<IMessageDTO, MessageDTO>();
            _container.RegisterType<IRelationshipDetailsDTO, RelationshipDetailsDTO>();

            _container.RegisterType<ITweetEntities, TweetEntitiesDTO>();
            _container.RegisterType<IUserEntities, UserEntities>();

            _container.RegisterType<IUrlEntity, UrlEntity>();
            _container.RegisterType<IHashtagEntity, HashtagEntity>();
            _container.RegisterType<IDescriptionEntity, DescriptionEntity>();
            _container.RegisterType<ISymbolEntity, SymbolEntity>();
        }

        private void InitializeHelpers()
        {
            _container.RegisterType<ITwitterStringFormatter, TwitterStringFormatter>();
        }

        private void InitializeQueryParameters()
        {
            _container.RegisterType<ITwitterListUpdateQueryParameters, TwitterListUpdateQueryParameters>();
            _container.RegisterType<IGetTweetsFromListQueryParameters, GetTweetsFromListQueryParameters>();
            _container.RegisterType<IUserTimelineQueryParameters, UserTimelineQueryParameters>();
            _container.RegisterType<IGetUserFavoritesQueryParameters, GetUserFavoritesQueryParameters>();
        }

        private void InitializeWrappers()
        {
            _container.RegisterType<IJObjectStaticWrapper, JObjectStaticWrapper>(RegistrationLifetime.InstancePerApplication);
            _container.RegisterType<IJsonConvertWrapper, JsonConvertWrapper>(RegistrationLifetime.InstancePerApplication);
        }

        private void InitializeExceptionHandler()
        {
            _container.RegisterType<IExceptionHandler, ExceptionHandler>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IWebExceptionInfoExtractor, WebExceptionInfoExtractor>(RegistrationLifetime.InstancePerApplication);          
            _container.RegisterType<ITwitterTimeoutException, TwitterTimeoutException>();
            _container.RegisterType<ITwitterExceptionInfo, TwitterExceptionInfo>();
        }

        private void InitializeSerialization()
        {
            _container.RegisterType<IJsonPropertyConverterRepository, JsonPropertyConverterRepository>();
            _container.RegisterType<IJsonObjectConverter, JsonObjectConverter>(RegistrationLifetime.InstancePerApplication);
        }
    }
}