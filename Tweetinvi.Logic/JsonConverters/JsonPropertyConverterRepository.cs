using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Core.Interfaces.Models.Entities.ExtendedEntities;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Logic.Model;
using Tweetinvi.Logic.TwitterEntities;
using Tweetinvi.Logic.TwitterEntities.ExtendedEntities;

namespace Tweetinvi.Logic.JsonConverters
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

            IntializeClassicalTypesConvertes();
            InitializeTweetinviObjectConverters();
            InitializeTweetinviInterfacesConverters();
            InitializeEntitiesConverters();
        }

        private static void IntializeClassicalTypesConvertes()
        {
            var nullableBoolConverter = new JsonTwitterNullableConverter<bool>();
            var nullableLongConverter = new JsonTwitterNullableConverter<long>();
            var nullableIntegerConverter = new JsonTwitterNullableConverter<int>();
            var nullableDoubleConverter = new JsonTwitterNullableConverter<double>();
            var dateTimeConverter = new JsonTwitterDateTimeConverter();

            JsonConverters.Add(typeof(bool), nullableBoolConverter);
            JsonConverters.Add(typeof(long), nullableLongConverter);
            JsonConverters.Add(typeof(int), nullableIntegerConverter);
            JsonConverters.Add(typeof(double), nullableDoubleConverter);
            JsonConverters.Add(typeof(DateTime), dateTimeConverter);
        }

        private static void InitializeTweetinviObjectConverters()
        {
            var privacyModeConverter = new JsonPrivacyModeConverter();
            var coordinatesConverter = new JsonInterfaceToObjectConverter<ICoordinates, CoordinatesDTO>();
            var languageConverter = new JsonLanguageConverter();
            var allowContributorRequestConverter = new JsonAllowContributorRequestConverter();
            var allowDirectMessage = new JsonAllowDirectMessagesConverter();

            JsonConverters.Add(typeof(PrivacyMode), privacyModeConverter);
            JsonConverters.Add(typeof(ICoordinates), coordinatesConverter);
            JsonConverters.Add(typeof(Language), languageConverter);
            JsonConverters.Add(typeof(AllowContributorRequestMode), allowContributorRequestConverter);
            JsonConverters.Add(typeof(AllowDirectMessagesFrom), allowDirectMessage);
        }

        private static void InitializeTweetinviInterfacesConverters()
        {
            var userConverter = new JsonInterfaceToObjectConverter<IUserDTO, UserDTO>();
            var userIdentifierConverter = new JsonInterfaceToObjectConverter<IUserIdentifier, UserIdentifierDTO>();
            var tweetConverter = new JsonInterfaceToObjectConverter<ITweetDTO, TweetDTO>();
            var tweetIdentifierConverter = new JsonInterfaceToObjectConverter<ITweetIdentifier, TweetIdentifierDTO>();
            var twitterListConverter = new JsonInterfaceToObjectConverter<ITwitterListDTO, TwitterListDTO>();
            var messageConverter = new JsonInterfaceToObjectConverter<IMessageDTO, MessageDTO>();
            var oembedTweetConverter = new JsonInterfaceToObjectConverter<IOEmbedTweetDTO, OEmbedTweetDTO>();
            var relationshipConverter = new JsonInterfaceToObjectConverter<IRelationshipDetailsDTO, RelationshipDetailsDTO>();
            var relationshipStateConverter = new JsonInterfaceToObjectConverter<IRelationshipStateDTO, RelationshipStateDTO>();
            var accountSettingsConverter = new JsonInterfaceToObjectConverter<IAccountSettingsDTO, AccountSettingsDTO>();
            var geoConverter = new JsonInterfaceToObjectConverter<IGeo, Geo>();
            var timezoneConverter = new JsonInterfaceToObjectConverter<ITimeZone, TimeZone>();
            var trendLocationConverter = new JsonInterfaceToObjectConverter<ITrendLocation, TrendLocation>();
            var placeConverter = new JsonInterfaceToObjectConverter<IPlace, Place>();
            var trendConverter = new JsonInterfaceToObjectConverter<ITrend, Trend>();
            var placeTrendsConverter = new JsonInterfaceToObjectConverter<IPlaceTrends, PlaceTrends>();
            var woeIdLocationConverter = new JsonInterfaceToObjectConverter<IWoeIdLocation, WoeIdLocation>();
            var tokenRateLimitConverter = new JsonInterfaceToObjectConverter<ITokenRateLimit, TokenRateLimit>();
            var tokenRateLimitsConverter = new JsonInterfaceToObjectConverter<ITokenRateLimits, TokenRateLimits>();
            var savedSearchConverter = new JsonInterfaceToObjectConverter<ISavedSearchDTO, SavedSearchDTO>();
            var searchMetadataConverter = new JsonInterfaceToObjectConverter<ISearchMetadata, SearchMetadata>();
            var tweetWithSearchMetadataConverter = new JsonInterfaceToObjectConverter<ITweetWithSearchMetadataDTO, TweetWithSearchMetadataDTO>();
            var tweetFromSearchMetadataConverter = new JsonInterfaceToObjectConverter<ITweetFromSearchMetadata, TweetFromSearchMetadata>();

            var uploadedImageDetailsConverter = new JsonInterfaceToObjectConverter<IUploadedImageDetails, UploadedImageDetailsDTO>();
            var uploadedVideoDetailsConverter = new JsonInterfaceToObjectConverter<IUploadedVideoDetails, UploadedVideoDetailsDTO>();

            var twitterConfigurationConverter = new JsonInterfaceToObjectConverter<ITwitterConfiguration, TwitterConfiguration>();

            JsonConverters.Add(typeof(IUserDTO), userConverter);
            JsonConverters.Add(typeof(IUserIdentifier), userIdentifierConverter);
            JsonConverters.Add(typeof(ITweetDTO), tweetConverter);
            JsonConverters.Add(typeof(ITweetIdentifier), tweetIdentifierConverter);
            JsonConverters.Add(typeof(ITwitterListDTO), twitterListConverter);
            JsonConverters.Add(typeof(IMessageDTO), messageConverter);
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
            JsonConverters.Add(typeof(IPlaceTrends), placeTrendsConverter);

            JsonConverters.Add(typeof(ITokenRateLimit), tokenRateLimitConverter);
            JsonConverters.Add(typeof(ITokenRateLimits), tokenRateLimitsConverter);
            JsonConverters.Add(typeof(ISavedSearch), savedSearchConverter);

            JsonConverters.Add(typeof(ISearchMetadata), searchMetadataConverter);
            JsonConverters.Add(typeof(ITweetWithSearchMetadataDTO), tweetWithSearchMetadataConverter);
            JsonConverters.Add(typeof(ITweetFromSearchMetadata), tweetFromSearchMetadataConverter);

            JsonConverters.Add(typeof(IUploadedImageDetails), uploadedImageDetailsConverter);
            JsonConverters.Add(typeof(IUploadedVideoDetails), uploadedVideoDetailsConverter);

            JsonConverters.Add(typeof(ITwitterConfiguration), twitterConfigurationConverter);
        }

        private static void InitializeEntitiesConverters()
        {
            var hashtagEntityConverter = new JsonInterfaceToObjectConverter<IHashtagEntity, HashtagEntity>();
            var urlEntityConverter = new JsonInterfaceToObjectConverter<IHashtagEntity, HashtagEntity>();
            var mediaEntityConverter = new JsonInterfaceToObjectConverter<IHashtagEntity, HashtagEntity>();
            var mediaEntitySizeConverter = new JsonInterfaceToObjectConverter<IMediaEntitySize, MediaEntitySize>();
            var descriptionEntityConverter = new JsonInterfaceToObjectConverter<IDescriptionEntity, DescriptionEntity>();
            var websiteEntityConverter = new JsonInterfaceToObjectConverter<IWebsiteEntity, WebsiteEntity>();

            var userEntitiesConverter = new JsonInterfaceToObjectConverter<IUserEntities, UserEntities>();
            var tweetEntitiesConverter = new JsonInterfaceToObjectConverter<ITweetEntities, TweetEntitiesDTO>();

            JsonConverters.Add(typeof(IHashtagEntity), hashtagEntityConverter);
            JsonConverters.Add(typeof(IUrlEntity), urlEntityConverter);
            JsonConverters.Add(typeof(IMediaEntity), mediaEntityConverter);
            JsonConverters.Add(typeof(IMediaEntitySize), mediaEntitySizeConverter);
            JsonConverters.Add(typeof(IDescriptionEntity), descriptionEntityConverter);
            JsonConverters.Add(typeof(IWebsiteEntity), websiteEntityConverter);

            JsonConverters.Add(typeof(IUserEntities), userEntitiesConverter);
            JsonConverters.Add(typeof(ITweetEntities), tweetEntitiesConverter);

            // Extended Entities
            var videoEntityVariantConverter = new JsonInterfaceToObjectConverter<IVideoEntityVariant, VideoEntityVariant>();
            var videoInformationEntityConverter = new JsonInterfaceToObjectConverter<IVideoInformationEntity, VideoInformationEntity>();

            JsonConverters.Add(typeof(IVideoEntityVariant), videoEntityVariantConverter);
            JsonConverters.Add(typeof(IVideoInformationEntity), videoInformationEntityConverter);
        }

        public JsonConverter GetObjectConverter(object objectToConvert)
        {
            return GetTypeConverter(objectToConvert.GetType());
        }

        public JsonConverter GetTypeConverter(Type objectType)
        {
            return JsonConverters[objectType];
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return GetTypeConverter(objectType).ReadJson(reader, objectType, existingValue, serializer);
        }

        public override bool CanConvert(Type objectType)
        {
            return JsonConverters.ContainsKey(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}