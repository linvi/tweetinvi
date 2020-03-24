using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.DTO.Events;
using Tweetinvi.Core.DTO.Webhooks;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.Models.Properties;
using Tweetinvi.Core.Models.TwitterEntities;
using Tweetinvi.Core.Models.TwitterEntities.ExtendedEntities;
using Tweetinvi.Core.Upload;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Events;
using Tweetinvi.Models.DTO.Webhooks;
using Tweetinvi.Models.Entities;
using Tweetinvi.Models.Entities.ExtendedEntities;

namespace Tweetinvi.Core.JsonConverters
{
    /// <summary>
    /// Repository of converters used to transform json into a collection of T
    /// </summary>
    public class JsonPropertiesConverterRepository
    {
        public static JsonConverter[] Converters { get; private set; }

        static JsonPropertiesConverterRepository()
        {
            Initialize();
        }

        private static void Initialize()
        {
            var converters = new List<JsonConverter>
            {
                new JsonInterfaceToObjectConverter<ITweetDTO, TweetDTO>(),
                new JsonInterfaceToObjectConverter<ITweetWithSearchMetadataDTO, TweetWithSearchMetadataDTO>(),
                new JsonInterfaceToObjectConverter<ITwitterListDTO, TwitterListDTO>(),
                new JsonInterfaceToObjectConverter<IOEmbedTweetDTO, OEmbedTweetDTO>(),
                new JsonInterfaceToObjectConverter<IUserDTO, UserDTO>(),
                new JsonInterfaceToObjectConverter<IUploadedMediaInfo, UploadedMediaInfo>(),
                new JsonInterfaceToObjectConverter<IUploadProcessingError, UploadProcessingError>(),

                new JsonInterfaceToObjectConverter<IRelationshipDetailsDTO, RelationshipDetailsDTO>(),
                new JsonInterfaceToObjectConverter<IRelationshipStateDTO, RelationshipStateDTO>(),

                new JsonInterfaceToObjectConverter<IAccountSettingsDTO, AccountSettingsDTO>(),
                new JsonInterfaceToObjectConverter<ILocation, Location>(),
                new JsonInterfaceToObjectConverter<IPlace, Place>(),

                new JsonInterfaceToObjectConverter<IUrlEntity, UrlEntity>(),
                new JsonInterfaceToObjectConverter<IHashtagEntity, HashtagEntity>(),
                new JsonInterfaceToObjectConverter<IMediaEntity, MediaEntity>(),
                new JsonInterfaceToObjectConverter<IMediaEntitySize, MediaEntitySize>(),
                new JsonInterfaceToObjectConverter<IUserMentionEntity, UserMentionEntity>(),
                new JsonInterfaceToObjectConverter<IDescriptionEntity, DescriptionEntity>(),
                new JsonInterfaceToObjectConverter<IWebsiteEntity, WebsiteEntity>(),
                new JsonInterfaceToObjectConverter<ISymbolEntity, SymbolEntity>(),

                new JsonInterfaceToObjectConverter<IUserEntities, UserEntities>(),

                new JsonInterfaceToObjectConverter<ITweetEntities, TweetEntitiesDTO>(),
                new JsonInterfaceToObjectConverter<IObjectEntities, ObjectEntitiesDTO>(),
                new JsonInterfaceToObjectConverter<IVideoEntityVariant, VideoEntityVariant>(),

                new JsonInterfaceToObjectConverter<IRelationshipDetails, RelationshipDetails>(),
                new JsonInterfaceToObjectConverter<IRelationshipState, RelationshipState>(),

                new JsonInterfaceToObjectConverter<IGetTrendsAtResult, GetTrendsAtResult>(),
                new JsonInterfaceToObjectConverter<ITrend, Trend>(),
                new JsonInterfaceToObjectConverter<ITrendLocation, TrendLocation>(),
                new JsonInterfaceToObjectConverter<IWoeIdLocation, WoeIdLocation>(),


                new JsonInterfaceToObjectConverter<IEndpointRateLimit, EndpointRateLimit>(),
                new JsonInterfaceToObjectConverter<ICredentialsRateLimits, CredentialsRateLimits>(),
                new JsonInterfaceToObjectConverter<ISavedSearchDTO, SavedSearchDTO>(),
                new JsonInterfaceToObjectConverter<ITwitterExceptionInfo, TwitterExceptionInfo>(),

                new JsonInterfaceToObjectConverter<ISearchResultsDTO, SearchResultsDTO>(),
                new JsonInterfaceToObjectConverter<ITwitterConfiguration, TwitterConfiguration>(),
                new JsonInterfaceToObjectConverter<ICategorySuggestion, CategorySuggestion>(),
                new JsonInterfaceToObjectConverter<IUrlEntity, UrlEntity>(),

                // JsonCoordinatesConverter is used only for Properties (with an s) and not Property
                // because Twitter does not provide the coordinates the same way if it is a list or
                // if it is a single argument.
                new JsonCoordinatesConverter(),

                new JsonInterfaceToObjectConverter<IQuickReplyOption, QuickReplyOption>(),
                new JsonInterfaceToObjectConverter<IQuickReplyDTO, QuickReplyDTO>(),
                new JsonInterfaceToObjectConverter<IApp, App>(),
                new JsonInterfaceToObjectConverter<IEventInitiatedViaDTO, EventInitiatedViaDTO>(),
                new JsonInterfaceToObjectConverter<IMessageDataDTO, MessageDataDTO>(),
                new JsonInterfaceToObjectConverter<IQuickReplyResponse, QuickReplyResponse>(),
                new JsonInterfaceToObjectConverter<IMessageCreateTargetDTO, MessageCreateTargetDTO>(),
                new JsonInterfaceToObjectConverter<IMessageEventDTO, MessageEventDTO>(),
                new JsonInterfaceToObjectConverter<IMessageCreateDTO, MessageCreateDTO>(),
                new JsonInterfaceToObjectConverter<IMessageEventWithAppDTO, MessageEventWithAppDTO>(),
                new JsonInterfaceToObjectConverter<IGetMessageDTO, GetMessageDTO>(),
                new JsonInterfaceToObjectConverter<ICreateMessageDTO, CreateMessageDTO>(),
                new JsonInterfaceToObjectConverter<IAttachmentDTO, AttachmentDTO>(),
                new JsonInterfaceToObjectConverter<IMessageEntities, MessageEntitiesDTO>(),

                new JsonInterfaceToObjectConverter<IUploadInitModel, UploadInitModel>(),

                // Webhooks
                new JsonInterfaceToObjectConverter<IWebhookDTO, WebhookDTO>(),
                new JsonInterfaceToObjectConverter<IWebhookEnvironmentDTO, WebhookEnvironmentDTO>(),
                new JsonInterfaceToObjectConverter<IGetAccountActivityWebhookEnvironmentsResultDTO, GetAccountActivityWebhookEnvironmentsResultDTO>(),
                new JsonInterfaceToObjectConverter<IWebhookSubscriptionsCount, WebhookSubscriptionsCountDTO>(),
                new JsonInterfaceToObjectConverter<IWebhookSubscriptionDTO, WebhookSubscriptionDTO>(),
                new JsonInterfaceToObjectConverter<IWebhookEnvironmentSubscriptionsDTO, WebhookEnvironmentSubscriptionsDTO>(),


                // Enums (that have JSON serialization implemented)
                new JsonEnumStringConverter<EventType>(),
                new JsonEnumStringConverter<QuickReplyType>(),
                new JsonEnumStringConverter<AttachmentType>(),
            };

            Converters = converters.ToArray();
        }

        public static void TryOverride<TInterface, TTo>() where TTo : TInterface
        {
            var converter = Converters.OfType<IJsonInterfaceToObjectConverter>().JustOneOrDefault(x => x.InterfaceType == typeof (TInterface));

            if (converter != null)
            {
                var converters = Converters.ToList();
                converters.Remove((JsonConverter)converter);
                converters.Add(new JsonInterfaceToObjectConverter<TInterface, TTo>());
                Converters = converters.ToArray();
            }
        }
    }
}