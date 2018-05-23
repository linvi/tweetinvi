using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Exceptions;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Logic.Exceptions;
using Tweetinvi.Logic.Helpers;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Logic.Model;
using Tweetinvi.Logic.QueryParameters;
using Tweetinvi.Logic.TwitterEntities;
using Tweetinvi.Logic.Wrapper;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;

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
            _container.RegisterType<ISearchQueryResult, SearchQueryResult>();
            _container.RegisterGeneric(typeof(IResultsWithCursor<>), typeof(ResultsWithCursor<>));
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
            _container.RegisterType<IRelationshipDetailsDTO, RelationshipDetailsDTO>();

            _container.RegisterType<ITweetEntities, TweetEntitiesDTO>();
            _container.RegisterType<IObjectEntities, ObjectEntitiesDTO>();
            _container.RegisterType<IUserEntities, UserEntities>();

            _container.RegisterType<IUrlEntity, UrlEntity>();
            _container.RegisterType<IHashtagEntity, HashtagEntity>();
            _container.RegisterType<IDescriptionEntity, DescriptionEntity>();
            _container.RegisterType<ISymbolEntity, SymbolEntity>();

            _container.RegisterType<IQuickReplyOption, QuickReplyOption>();
            _container.RegisterType<IQuickReplyDTO, QuickReplyDTO>();
            _container.RegisterType<IApp, App>();
            _container.RegisterType<IEventInitiatedViaDTO, EventInitiatedViaDTO>();
            _container.RegisterType<IMessageDataDTO, MessageDataDTO>();
            _container.RegisterType<IQuickReplyResponse, QuickReplyResponse>();
            _container.RegisterType<IMessageCreateTargetDTO, MessageCreateTargetDTO>();
            _container.RegisterType<IEventDTO, EventDTO>();
            _container.RegisterType<IMessageCreateDTO, MessageCreateDTO>();
            _container.RegisterType<IGetMessageDTO, GetMessageDTO>();
            _container.RegisterType<IGetMessagesDTO, GetMessagesDTO>();
            _container.RegisterType<ICreateMessageDTO, CreateMessageDTO>();
            _container.RegisterType<IAttachmentDTO, AttachmentDTO>();
            _container.RegisterType<IMessageEntities, MessageEntitiesDTO>();


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