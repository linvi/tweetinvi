using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.DTO.Events;
using Tweetinvi.Core.DTO.Webhooks;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.Models.Properties;
using Tweetinvi.Core.Models.TwitterEntities;
using Tweetinvi.Core.Models.TwitterEntities.ExtendedEntities;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Events;
using Tweetinvi.Models.DTO.Webhooks;
using Tweetinvi.Models.Entities;
using Tweetinvi.Models.Entities.ExtendedEntities;
using TimeZone = Tweetinvi.Core.Models.Properties.TimeZone;

namespace Tweetinvi.Core.JsonConverters
{
    /// <summary>
    /// Repository of converters used to transform json into a specific type T.
    /// It should be used as a Field attribute (e.g. [JsonConverter(typeof(JsonPropertyConverterRepository))])
    /// </summary>
    public class JsonPropertyConverterRepository : JsonConverter, IJsonPropertyConverterRepository
    {
        public static readonly Dictionary<Type, JsonConverter> JsonConverters;

        static JsonPropertyConverterRepository()
        {
            JsonConverters = new Dictionary<Type, JsonConverter>();

            IntializeClassicalTypesConverters();
            InitializeTweetinviObjectConverters();
            InitializeTweetinviInterfacesConverters();
            InitializeEntitiesConverters();
            InitializeWebhookConverters();
        }

        private static void IntializeClassicalTypesConverters()
        {
            var nullableBoolConverter = new JsonTwitterNullableConverter<bool>();
            var nullableLongConverter = new JsonTwitterNullableConverter<long>();
            var nullableIntegerConverter = new JsonTwitterNullableConverter<int>();
            var nullableDoubleConverter = new JsonTwitterNullableConverter<double>();
            var dateTimeConverter = new JsonTwitterDateTimeConverter();
            var dateTimeOffsetConverter = new JsonTwitterDateTimeOffsetConverter();

            JsonConverters.Add(typeof(bool), nullableBoolConverter);
            JsonConverters.Add(typeof(long), nullableLongConverter);
            JsonConverters.Add(typeof(long?), nullableLongConverter);
            JsonConverters.Add(typeof(int), nullableIntegerConverter);
            JsonConverters.Add(typeof(double), nullableDoubleConverter);
            JsonConverters.Add(typeof(DateTime), dateTimeConverter);
            JsonConverters.Add(typeof(DateTimeOffset), dateTimeOffsetConverter);
        }

        private static void InitializeTweetinviObjectConverters()
        {
            var privacyModeConverter = new JsonPrivacyModeConverter();
            var coordinatesConverter = new JsonInterfaceToObjectConverter<ICoordinates, CoordinatesDTO>();
            var languageConverter = new JsonLanguageConverter();
            var allowContributorRequestConverter = new JsonAllowContributorRequestConverter();
            var allowDirectMessagesConverter = new JsonAllowDirectMessagesConverter();
            var quickReplyTypeConverter = new JsonEnumStringConverter<QuickReplyType>();
            var eventTypeConverter = new JsonEnumStringConverter<EventType>();
            var attachmentTypeConverter = new JsonEnumStringConverter<AttachmentType>();

            JsonConverters.Add(typeof(PrivacyMode), privacyModeConverter);
            JsonConverters.Add(typeof(ICoordinates), coordinatesConverter);
            JsonConverters.Add(typeof(Language), languageConverter);
            JsonConverters.Add(typeof(Language?), languageConverter);
            JsonConverters.Add(typeof(AllowContributorRequestMode), allowContributorRequestConverter);
            JsonConverters.Add(typeof(AllowDirectMessagesFrom), allowDirectMessagesConverter);
            JsonConverters.Add(typeof(QuickReplyType), quickReplyTypeConverter);
            JsonConverters.Add(typeof(EventType), eventTypeConverter);
            JsonConverters.Add(typeof(AttachmentType), attachmentTypeConverter);
        }

        private static void InitializeTweetinviInterfacesConverters()
        {
            var userDTOConverter = new JsonInterfaceToObjectConverter<IUserDTO, UserDTO>();
            var userIdentifierConverter = new JsonInterfaceToObjectConverter<IUserIdentifier, UserIdentifierDTO>();
            var tweetConverter = new JsonInterfaceToObjectConverter<ITweetDTO, TweetDTO>();
            var extendedTweetDTOConverter = new JsonInterfaceToObjectConverter<IExtendedTweet, ExtendedTweet>();
            var tweetIdentifierConverter = new JsonInterfaceToObjectConverter<ITweetIdentifier, TweetIdentifierDTO>();
            var twitterListConverter = new JsonInterfaceToObjectConverter<ITwitterListDTO, TwitterListDTO>();
            var oembedTweetConverter = new JsonInterfaceToObjectConverter<IOEmbedTweetDTO, OEmbedTweetDTO>();
            var relationshipConverter = new JsonInterfaceToObjectConverter<IRelationshipDetailsDTO, RelationshipDetailsDTO>();
            var relationshipStateConverter = new JsonInterfaceToObjectConverter<IRelationshipStateDTO, RelationshipStateDTO>();
            var accountSettingsConverter = new JsonInterfaceToObjectConverter<IAccountSettingsDTO, AccountSettingsDTO>();
            var geoConverter = new JsonInterfaceToObjectConverter<IGeo, Geo>();
            var timezoneConverter = new JsonInterfaceToObjectConverter<ITimeZone, TimeZone>();
            var trendLocationConverter = new JsonInterfaceToObjectConverter<ITrendLocation, TrendLocation>();
            var placeConverter = new JsonInterfaceToObjectConverter<IPlace, Place>();
            var trendConverter = new JsonInterfaceToObjectConverter<ITrend, Trend>();
            var placeTrendsConverter = new JsonInterfaceToObjectConverter<IGetTrendsAtResult, GetTrendsAtResult>();
            var woeIdLocationConverter = new JsonInterfaceToObjectConverter<IWoeIdLocation, WoeIdLocation>();
            var endpointRateLimitConverter = new JsonInterfaceToObjectConverter<IEndpointRateLimit, EndpointRateLimit>();
            var credentialsRateLimitsConverter = new JsonInterfaceToObjectConverter<ICredentialsRateLimits, CredentialsRateLimits>();
            var savedSearchConverter = new JsonInterfaceToObjectConverter<ISavedSearchDTO, SavedSearchDTO>();
            var searchMetadataConverter = new JsonInterfaceToObjectConverter<ISearchMetadata, SearchMetadata>();
            var tweetWithSearchMetadataConverter = new JsonInterfaceToObjectConverter<ITweetWithSearchMetadataDTO, TweetWithSearchMetadataDTO>();
            var tweetFromSearchMetadataConverter = new JsonInterfaceToObjectConverter<ITweetFromSearchMetadata, TweetFromSearchMetadata>();

            var uploadedImageDetailsConverter = new JsonInterfaceToObjectConverter<IUploadedImageDetails, UploadedImageDetailsDTO>();
            var uploadedVideoDetailsConverter = new JsonInterfaceToObjectConverter<IUploadedVideoDetails, UploadedVideoDetailsDTO>();
            var uploadProcessingInfoConverter = new JsonInterfaceToObjectConverter<IUploadProcessingInfo, UploadProcessingInfo>();
            var uploadProcessingErrorConverter = new JsonInterfaceToObjectConverter<IUploadProcessingError, UploadProcessingError>();

            var twitterConfigurationConverter = new JsonInterfaceToObjectConverter<ITwitterConfiguration, TwitterConfiguration>();

            var quickReplyOptionConverter = new JsonInterfaceToObjectConverter<IQuickReplyOption, QuickReplyOption>();
            var quickReplyConverter = new JsonInterfaceToObjectConverter<IQuickReplyDTO, QuickReplyDTO>();
            var appConverter = new JsonInterfaceToObjectConverter<IApp, App>();
            var eventInitiatedViaConverter = new JsonInterfaceToObjectConverter<IEventInitiatedViaDTO, EventInitiatedViaDTO>();
            var messageDataConverter = new JsonInterfaceToObjectConverter<IMessageDataDTO, MessageDataDTO>();
            var quickReplyResponseConverter = new JsonInterfaceToObjectConverter<IQuickReplyResponse, QuickReplyResponse>();
            var messageCreateTargetConverter = new JsonInterfaceToObjectConverter<IMessageCreateTargetDTO, MessageCreateTargetDTO>();
            var eventConverter = new JsonInterfaceToObjectConverter<IMessageEventDTO, MessageEventDTO>();
            var messageCreateConverter = new JsonInterfaceToObjectConverter<IMessageCreateDTO, MessageCreateDTO>();
            var getMessageConverter = new JsonInterfaceToObjectConverter<IGetMessageDTO, GetMessageDTO>();
            var createMessageConverter = new JsonInterfaceToObjectConverter<ICreateMessageDTO, CreateMessageDTO>();
            var attachmentConverter = new JsonInterfaceToObjectConverter<IAttachmentDTO, AttachmentDTO>();
            var messageEntitiesConverter = new JsonInterfaceToObjectConverter<IMessageEntities, MessageEntitiesDTO>();

            JsonConverters.Add(typeof(IUserDTO), userDTOConverter);
            JsonConverters.Add(typeof(IUserIdentifier), userIdentifierConverter);
            JsonConverters.Add(typeof(ITweetDTO), tweetConverter);
            JsonConverters.Add(typeof(IExtendedTweet), extendedTweetDTOConverter);
            JsonConverters.Add(typeof(ITweetIdentifier), tweetIdentifierConverter);
            JsonConverters.Add(typeof(ITwitterListDTO), twitterListConverter);
            JsonConverters.Add(typeof(IOEmbedTweetDTO), oembedTweetConverter);
            JsonConverters.Add(typeof(IRelationshipDetailsDTO), relationshipConverter);
            JsonConverters.Add(typeof(IRelationshipStateDTO), relationshipStateConverter);
            JsonConverters.Add(typeof(IAccountSettingsDTO), accountSettingsConverter);

            JsonConverters.Add(typeof(IGeo), geoConverter);
            JsonConverters.Add(typeof(ITimeZone), timezoneConverter);
            JsonConverters.Add(typeof(ITrendLocation), trendLocationConverter);
            JsonConverters.Add(typeof(IPlace), placeConverter);
            JsonConverters.Add(typeof(IWoeIdLocation), woeIdLocationConverter);

            JsonConverters.Add(typeof(ITrend), trendConverter);
            JsonConverters.Add(typeof(IGetTrendsAtResult), placeTrendsConverter);

            JsonConverters.Add(typeof(IEndpointRateLimit), endpointRateLimitConverter);
            JsonConverters.Add(typeof(ICredentialsRateLimits), credentialsRateLimitsConverter);
            JsonConverters.Add(typeof(ISavedSearch), savedSearchConverter);

            JsonConverters.Add(typeof(ISearchMetadata), searchMetadataConverter);
            JsonConverters.Add(typeof(ITweetWithSearchMetadataDTO), tweetWithSearchMetadataConverter);
            JsonConverters.Add(typeof(ITweetFromSearchMetadata), tweetFromSearchMetadataConverter);

            JsonConverters.Add(typeof(IUploadedImageDetails), uploadedImageDetailsConverter);
            JsonConverters.Add(typeof(IUploadedVideoDetails), uploadedVideoDetailsConverter);
            JsonConverters.Add(typeof(IUploadProcessingInfo), uploadProcessingInfoConverter);
            JsonConverters.Add(typeof(IUploadProcessingError), uploadProcessingErrorConverter);

            JsonConverters.Add(typeof(ITwitterConfiguration), twitterConfigurationConverter);

            JsonConverters.Add(typeof(IQuickReplyOption), quickReplyOptionConverter);
            JsonConverters.Add(typeof(IQuickReplyDTO), quickReplyConverter);
            JsonConverters.Add(typeof(IApp), appConverter);
            JsonConverters.Add(typeof(IEventInitiatedViaDTO), eventInitiatedViaConverter);
            JsonConverters.Add(typeof(IMessageDataDTO), messageDataConverter);
            JsonConverters.Add(typeof(IQuickReplyResponse), quickReplyResponseConverter);
            JsonConverters.Add(typeof(IMessageCreateTargetDTO), messageCreateTargetConverter);
            JsonConverters.Add(typeof(IMessageEventDTO), eventConverter);
            JsonConverters.Add(typeof(IMessageCreateDTO), messageCreateConverter);
            JsonConverters.Add(typeof(IGetMessageDTO), getMessageConverter);
            JsonConverters.Add(typeof(ICreateMessageDTO), createMessageConverter);
            JsonConverters.Add(typeof(IAttachmentDTO), attachmentConverter);
            JsonConverters.Add(typeof(IMessageEntities), messageEntitiesConverter);
        }

        private static void InitializeEntitiesConverters()
        {
            var hashtagEntityConverter = new JsonInterfaceToObjectConverter<IHashtagEntity, HashtagEntity>();
            var urlEntityConverter = new JsonInterfaceToObjectConverter<IUrlEntity, UrlEntity>();
            var mediaEntityConverter = new JsonInterfaceToObjectConverter<IMediaEntity, MediaEntity>();
            var mediaEntitySizeConverter = new JsonInterfaceToObjectConverter<IMediaEntitySize, MediaEntitySize>();
            var descriptionEntityConverter = new JsonInterfaceToObjectConverter<IDescriptionEntity, DescriptionEntity>();
            var websiteEntityConverter = new JsonInterfaceToObjectConverter<IWebsiteEntity, WebsiteEntity>();

            var userEntitiesConverter = new JsonInterfaceToObjectConverter<IUserEntities, UserEntities>();
            var tweetEntitiesConverter = new JsonInterfaceToObjectConverter<ITweetEntities, TweetEntitiesDTO>();
            var objectEntitiesConverter = new JsonInterfaceToObjectConverter<IObjectEntities, ObjectEntitiesDTO>();

            JsonConverters.Add(typeof(IHashtagEntity), hashtagEntityConverter);
            JsonConverters.Add(typeof(IUrlEntity), urlEntityConverter);
            JsonConverters.Add(typeof(IMediaEntity), mediaEntityConverter);
            JsonConverters.Add(typeof(IMediaEntitySize), mediaEntitySizeConverter);
            JsonConverters.Add(typeof(IDescriptionEntity), descriptionEntityConverter);
            JsonConverters.Add(typeof(IWebsiteEntity), websiteEntityConverter);

            JsonConverters.Add(typeof(IUserEntities), userEntitiesConverter);
            JsonConverters.Add(typeof(ITweetEntities), tweetEntitiesConverter);
            JsonConverters.Add(typeof(IObjectEntities), objectEntitiesConverter);

            // Extended Entities
            var videoEntityVariantConverter = new JsonInterfaceToObjectConverter<IVideoEntityVariant, VideoEntityVariant>();
            var videoInformationEntityConverter = new JsonInterfaceToObjectConverter<IVideoInformationEntity, VideoInformationEntity>();

            JsonConverters.Add(typeof(IVideoEntityVariant), videoEntityVariantConverter);
            JsonConverters.Add(typeof(IVideoInformationEntity), videoInformationEntityConverter);
        }

        private static void InitializeWebhookConverters()
        {
            var webhookDTOConverter = new JsonInterfaceToObjectConverter<IWebhookDTO, WebhookDTO>();
            var webhookEnvironmentDTOConverter = new JsonInterfaceToObjectConverter<IWebhookEnvironmentDTO, WebhookEnvironmentDTO>();
            var getAllWebhooksResultDTOConverter = new JsonInterfaceToObjectConverter<IGetAccountActivityWebhookEnvironmentsResultDTO, GetAccountActivityWebhookEnvironmentsResultDTO>();
            var getWebhookSubscriptionsCountResultDTOConverter = new JsonInterfaceToObjectConverter<IWebhookSubscriptionsCount, WebhookSubscriptionsCountDTO>();

            JsonConverters.Add(typeof(IWebhookDTO), webhookDTOConverter);
            JsonConverters.Add(typeof(IWebhookEnvironmentDTO), webhookEnvironmentDTOConverter);
            JsonConverters.Add(typeof(IGetAccountActivityWebhookEnvironmentsResultDTO), getAllWebhooksResultDTOConverter);
            JsonConverters.Add(typeof(IWebhookSubscriptionsCount), getWebhookSubscriptionsCountResultDTOConverter);
        }

        private static readonly object _convertersLocker = new object();
        public static void TryOverride<TInterface, TTo>() where TTo : TInterface
        {
            lock (_convertersLocker)
            {
                var jsonInterfaceToObjectConverter = JsonConverters.Where(x => x.Value is IJsonInterfaceToObjectConverter);
                var matchingConverter = jsonInterfaceToObjectConverter.Where(x => ((IJsonInterfaceToObjectConverter)x.Value).InterfaceType == typeof(TInterface)).ToArray();

                if (matchingConverter.Length == 1)
                {
                    JsonConverters.Remove(typeof(TInterface));
                    JsonConverters.Add(typeof(TInterface), new JsonInterfaceToObjectConverter<TInterface, TTo>());
                }
            }
        }

        public JsonConverter GetObjectConverter(object objectToConvert)
        {
            return GetTypeConverter(objectToConvert.GetType());
        }

        public JsonConverter GetTypeConverter(Type objectType)
        {
            lock (_convertersLocker)
            {
                return JsonConverters[objectType];
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return GetTypeConverter(objectType).ReadJson(reader, objectType, existingValue, serializer);
        }

        public override bool CanConvert(Type objectType)
        {
            lock (_convertersLocker)
            {
                return JsonConverters.ContainsKey(objectType);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}