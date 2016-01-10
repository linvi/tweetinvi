using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Core.Interfaces.Models.Entities.ExtendedEntities;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Logic.Exceptions;
using Tweetinvi.Logic.Model;
using Tweetinvi.Logic.TwitterEntities;
using Tweetinvi.Logic.TwitterEntities.ExtendedEntities;

namespace Tweetinvi.Logic.JsonConverters
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
                new JsonInterfaceToObjectConverter<IMessageDTO, MessageDTO>(),
                new JsonInterfaceToObjectConverter<IUploadedMediaInfo, UploadedMediaInfo>(),

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
                new JsonInterfaceToObjectConverter<IVideoEntityVariant, VideoEntityVariant>(),

                new JsonInterfaceToObjectConverter<IRelationshipDetails, RelationshipDetails>(),
                new JsonInterfaceToObjectConverter<IRelationshipState, RelationshipState>(),
                
                new JsonInterfaceToObjectConverter<IPlaceTrends, PlaceTrends>(),
                new JsonInterfaceToObjectConverter<ITrend, Trend>(),
                new JsonInterfaceToObjectConverter<ITrendLocation, TrendLocation>(),
                new JsonInterfaceToObjectConverter<IWoeIdLocation, WoeIdLocation>(),
                
                
                new JsonInterfaceToObjectConverter<ITokenRateLimit, TokenRateLimit>(),
                new JsonInterfaceToObjectConverter<ITokenRateLimits, TokenRateLimits>(),
                new JsonInterfaceToObjectConverter<ISavedSearchDTO, SavedSearchDTO>(),
                new JsonInterfaceToObjectConverter<ITwitterExceptionInfo, TwitterExceptionInfo>(),
                
                new JsonInterfaceToObjectConverter<ISearchResultsDTO, SearchResultsDTO>(),
                new JsonInterfaceToObjectConverter<ITwitterConfiguration, TwitterConfiguration>(),
                new JsonInterfaceToObjectConverter<ICategorySuggestion, CategorySuggestion>(),

                // JsonCoordinatesConverter is used only for Properties (with an s) and not Property
                // because Twitter does not provide the coordinates the same way if it is a list or
                // if it is a single argument.
                new JsonCoordinatesConverter(),
            };

            Converters = converters.ToArray();
        }

        public static void TryOverride<T, U>() where U : T
        {
            var converter = Converters.OfType<IJsonInterfaceToObjectConverter>().JustOneOrDefault(x => x.InterfaceType == typeof (T));

            if (converter != null)
            {
                var converters = Converters.ToList();
                converters.Remove((JsonConverter)converter);
                converters.Add(new JsonInterfaceToObjectConverter<T, U>());
                Converters = converters.ToArray();
            }
        }
    }
}