using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Models;
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
        public void Initialize(ITweetinviContainer container)
        {
            InitializeTwitterModels(container);
            InitializeTweetinviModels(container);

            InitializeDTOs(container);
            InitializeHelpers(container);
            InitializeWrappers(container);
            InitializeQueryParameters(container);
            InitializeExceptionHandler(container);
            InitializeSerialization(container);
        }

        // Initialize Models that are not objects coming from Twitter
        private void InitializeTweetinviModels(ITweetinviContainer container)
        {
            container.RegisterType<IMedia, Media>();
            container.RegisterType<IEditableMedia, Media>();
            container.RegisterType<ISearchQueryResult, SearchQueryResult>();
            container.RegisterGeneric(typeof(IResultsWithCursor<>), typeof(ResultsWithCursor<>));
        }

        // Initialize Models that are Twitter objects
        private void InitializeTwitterModels(ITweetinviContainer container)
        {
            container.RegisterType<ITweet, Tweet>();
            container.RegisterType<ITweetWithSearchMetadata, TweetWithSearchMetadata>();
            container.RegisterType<IOEmbedTweet, OEmbedTweet>();

            container.RegisterType<IUser, User>();
            container.RegisterType<IAuthenticatedUser, AuthenticatedUser>();

            container.RegisterType<ITwitterList, TwitterList>();

            container.RegisterType<ICoordinates, CoordinatesDTO>();
            container.RegisterType<ILocation, Location>();

            container.RegisterType<IAccountSettings, AccountSettings>();
            container.RegisterType<IMessage, Message>();
            container.RegisterType<IMention, Mention>();
            container.RegisterType<IRelationshipDetails, RelationshipDetails>();
            container.RegisterType<IRelationshipState, RelationshipState>();
            container.RegisterType<ISavedSearch, SavedSearch>();
        }

        private void InitializeDTOs(ITweetinviContainer container)
        {
            container.RegisterType<ITweetDTO, TweetDTO>();
            container.RegisterType<ITwitterListDTO, TwitterListDTO>();
            container.RegisterType<IUserDTO, UserDTO>();
            container.RegisterType<IRelationshipDetailsDTO, RelationshipDetailsDTO>();

            container.RegisterType<ITweetEntities, TweetEntitiesDTO>();
            container.RegisterType<IObjectEntities, ObjectEntitiesDTO>();
            container.RegisterType<IUserEntities, UserEntities>();

            container.RegisterType<IUrlEntity, UrlEntity>();
            container.RegisterType<IHashtagEntity, HashtagEntity>();
            container.RegisterType<IDescriptionEntity, DescriptionEntity>();
            container.RegisterType<ISymbolEntity, SymbolEntity>();

            container.RegisterType<IQuickReplyOption, QuickReplyOption>();
            container.RegisterType<IQuickReplyDTO, QuickReplyDTO>();
            container.RegisterType<IApp, App>();
            container.RegisterType<IEventInitiatedViaDTO, EventInitiatedViaDTO>();
            container.RegisterType<IMessageDataDTO, MessageDataDTO>();
            container.RegisterType<IQuickReplyResponse, QuickReplyResponse>();
            container.RegisterType<IMessageCreateTargetDTO, MessageCreateTargetDTO>();
            container.RegisterType<IMessageEventDTO, MessageEventDTO>();
            container.RegisterType<IMessageCreateDTO, MessageCreateDTO>();
            container.RegisterType<IGetMessageDTO, GetMessageDTO>();
            container.RegisterType<IGetMessagesDTO, GetMessagesDTO>();
            container.RegisterType<ICreateMessageDTO, CreateMessageDTO>();
            container.RegisterType<IAttachmentDTO, AttachmentDTO>();
            container.RegisterType<IMessageEntities, MessageEntitiesDTO>();
            container.RegisterType<IMediaEntity, MediaEntity>();
        }

        private void InitializeHelpers(ITweetinviContainer container)
        {
            container.RegisterType<ITwitterStringFormatter, TwitterStringFormatter>();
        }

        private void InitializeQueryParameters(ITweetinviContainer container)
        {
            container.RegisterType<ITwitterListUpdateQueryParameters, TwitterListUpdateQueryParameters>();
            container.RegisterType<IGetTweetsFromListQueryParameters, GetTweetsFromListQueryParameters>();
            container.RegisterType<IUserTimelineQueryParameters, UserTimelineQueryParameters>();
            container.RegisterType<IGetUserFavoritesQueryParameters, GetUserFavoritesQueryParameters>();
        }

        private void InitializeWrappers(ITweetinviContainer container)
        {
            container.RegisterType<IJObjectStaticWrapper, JObjectStaticWrapper>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IJsonConvertWrapper, JsonConvertWrapper>(RegistrationLifetime.InstancePerApplication);
        }

        private void InitializeExceptionHandler(ITweetinviContainer container)
        {
            container.RegisterType<IExceptionHandler, ExceptionHandler>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<IWebExceptionInfoExtractor, WebExceptionInfoExtractor>(RegistrationLifetime.InstancePerApplication);          
            container.RegisterType<ITwitterTimeoutException, TwitterTimeoutException>();
            container.RegisterType<ITwitterExceptionInfo, TwitterExceptionInfo>();
        }

        private void InitializeSerialization(ITweetinviContainer container)
        {
            container.RegisterType<IJsonPropertyConverterRepository, JsonPropertyConverterRepository>();
            container.RegisterType<IJsonObjectConverter, JsonObjectConverter>(RegistrationLifetime.InstancePerApplication);
        }
    }
}