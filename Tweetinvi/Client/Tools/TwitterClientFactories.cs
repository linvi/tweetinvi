using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.Models.Properties;
using Tweetinvi.Core.Models.TwitterEntities;
using Tweetinvi.Logic;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Events;

namespace Tweetinvi.Client.Tools
{
    public class TwitterClientFactories : ITwitterClientFactories
    {
        private readonly ITwitterClient _client;

        public TwitterClientFactories(ITwitterClient client)
        {
            _client = client;
        }

        public ITwitterList CreateTwitterList(string json)
        {
            var listDTO = _client.Json.DeserializeObject<ITwitterListDTO>(json);
            return CreateTwitterList(listDTO);
        }

        public ITwitterList CreateTwitterList(ITwitterListDTO twitterListDTO)
        {
            if (twitterListDTO == null)
            {
                return null;
            }

            return new TwitterList(twitterListDTO, _client);
        }

        public ITwitterList[] CreateTwitterLists(IEnumerable<ITwitterListDTO> listDTOs)
        {
            return listDTOs?.Select(CreateTwitterList).ToArray();
        }

        public IMessage CreateMessage(IMessageEventWithAppDTO messageEventWithAppDTO)
        {
            return new Core.Models.Message(messageEventWithAppDTO.MessageEvent, messageEventWithAppDTO.App, _client);
        }

        public IMessage[] CreateMessages(IEnumerable<IMessageEventWithAppDTO> eventWithAppDTOs)
        {
            return eventWithAppDTOs?.Select(CreateMessage).ToArray();
        }

        public IMessage CreateMessage(IGetMessageDTO getMessageDTO)
        {
            return _buildMessage(getMessageDTO.MessageEvent, getMessageDTO.Apps);
        }

        public IMessage CreateMessage(ICreateMessageDTO createMessageDTO)
        {
            return CreateMessage(createMessageDTO.MessageEvent);
        }

        public IMessage CreateMessage(IMessageEventDTO messageEventDTO)
        {
            return new Core.Models.Message(messageEventDTO, null, _client);
        }

        public IMessage CreateMessage(IMessageEventDTO messageEventDTO, IApp app)
        {
            return new Core.Models.Message(messageEventDTO, app, _client);
        }

        public IMessage CreateMessage(string json)
        {
            var eventWithAppDTO = _client.Json.DeserializeObject<IMessageEventWithAppDTO>(json);

            if (eventWithAppDTO.MessageEvent == null)
            {
                return null;
            }

            return CreateMessage(eventWithAppDTO);
        }

        private IMessage _buildMessage(IMessageEventDTO messageEventDTO, IDictionary<long, IApp> apps)
        {
            if (messageEventDTO.Type != EventType.MessageCreate)
            {
                return null;
            }

            // Get the app that was used to send this message.
            //  Note that we don't always get the App ID.
            //  Also assume that some apps could be missing from the dictionary.
            IApp app = null;
            if (messageEventDTO.MessageCreate.SourceAppId != null)
            {
                apps.TryGetValue(messageEventDTO.MessageCreate.SourceAppId.Value, out app);
            }

            return new Core.Models.Message(messageEventDTO, app, _client);
        }

        public IRelationshipState CreateRelationshipState(string json)
        {
            var dto = _client.Json.DeserializeObject<IRelationshipStateDTO>(json);
            return CreateRelationshipState(dto);
        }

        public IRelationshipState CreateRelationshipState(IRelationshipStateDTO relationshipStateDTO)
        {
            return relationshipStateDTO == null ? null : new RelationshipState(relationshipStateDTO);
        }

        public IRelationshipDetails CreateRelationshipDetails(string json)
        {
            var dto = _client.Json.DeserializeObject<IRelationshipDetailsDTO>(json);
            return CreateRelationshipDetails(dto);
        }

        public IRelationshipDetails CreateRelationshipDetails(IRelationshipDetailsDTO dto)
        {
            return dto == null ? null : new RelationshipDetails(dto);
        }

        public ISavedSearch CreateSavedSearch(string json)
        {
            var dto = _client.Json.DeserializeObject<ISavedSearchDTO>(json);
            return CreateSavedSearch(dto);
        }

        public ISavedSearch CreateSavedSearch(ISavedSearchDTO savedSearchDTO)
        {
            return savedSearchDTO == null ? null : new Core.Models.SavedSearch(savedSearchDTO);
        }

        public ISearchResult CreateSearchResult(ISearchResultsDTO[] searchResultsDTO)
        {
            var searchResults = searchResultsDTO?.Select(CreateSearchQueryResult).ToArray();
            return new SearchResult(searchResults);
        }

        public ISearchQueryResult CreateSearchQueryResult(ISearchResultsDTO searchResultsDTO)
        {
            var tweets = searchResultsDTO?.TweetDTOs?.Select(CreateTweetWithSearchMetadata);
            var matchingTweets = searchResultsDTO?.MatchingTweetDTOs?.Select(CreateTweetWithSearchMetadata);

            return new SearchQueryResult(tweets, matchingTweets, searchResultsDTO?.SearchMetadata);
        }

        public ITweet CreateTweet(string json)
        {
            var tweetDTO = _client.Json.DeserializeObject<ITweetDTO>(json);
            return CreateTweet(tweetDTO);
        }

        public ITweet CreateTweet(ITweetDTO tweetDTO)
        {
            if (tweetDTO == null)
            {
                return null;
            }

            return new Tweet(tweetDTO, _client.ClientSettings.TweetMode, _client);
        }

        public ITweetWithSearchMetadata CreateTweetWithSearchMetadata(ITweetWithSearchMetadataDTO tweetDTO)
        {
            if (tweetDTO == null)
            {
                return null;
            }

            return new TweetWithSearchMetadata(tweetDTO, _client.ClientSettings.TweetMode, _client);
        }

        public IOEmbedTweet CreateOEmbedTweet(string json)
        {
            var dto = _client.Json.DeserializeObject<IOEmbedTweetDTO>(json);
            return CreateOEmbedTweet(dto);
        }

        public IOEmbedTweet CreateOEmbedTweet(IOEmbedTweetDTO oEmbedTweetDTO)
        {
            if (oEmbedTweetDTO == null)
            {
                return null;
            }

            return new OEmbedTweet(oEmbedTweetDTO);
        }

        public IUser CreateUser(string json)
        {
            var tweetDTO = _client.Json.DeserializeObject<IUserDTO>(json);
            return CreateUser(tweetDTO);
        }

        public IUser CreateUser(IUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return null;
            }

            return new User(userDTO, _client);
        }

        public IUser[] CreateUsers(IEnumerable<IUserDTO> usersDTO)
        {
            return usersDTO?.Select(CreateUser).ToArray();
        }

        public IAuthenticatedUser CreateAuthenticatedUser(string json)
        {
            var tweetDTO = _client.Json.DeserializeObject<IUserDTO>(json);
            return CreateAuthenticatedUser(tweetDTO);
        }

        public IAuthenticatedUser CreateAuthenticatedUser(IUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return null;
            }

            return new AuthenticatedUser(userDTO, _client);
        }

        public IAccountSettings GenerateAccountSettingsFromJson(string json)
        {
            var accountSettingsDTO = _client.Json.DeserializeObject<IAccountSettingsDTO>(json);

            if (accountSettingsDTO == null)
            {
                return null;
            }

            return new AccountSettings(accountSettingsDTO);
        }
    }
}