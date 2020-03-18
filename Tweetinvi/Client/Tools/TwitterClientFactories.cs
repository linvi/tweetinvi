using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Helpers;
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
        private IJsonObjectConverter _jsonObjectConverter;

        public TwitterClientFactories(ITwitterClient client, IJsonObjectConverter jsonObjectConverter)
        {
            _client = client;
            _jsonObjectConverter = jsonObjectConverter;
        }

        public IAccountSettings CreateAccountSettings(IAccountSettingsDTO dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new AccountSettings(dto);
        }

        public ITwitterList CreateTwitterList(string json)
        {
            var listDTO = _jsonObjectConverter.Deserialize<ITwitterListDTO>(json);
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
            return new Message(messageEventWithAppDTO.MessageEvent, messageEventWithAppDTO.App, _client);
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
            return CreateMessage(createMessageDTO?.MessageEvent);
        }

        public IMessage CreateMessage(IMessageEventDTO messageEventDTO)
        {
            if (messageEventDTO == null)
            {
                return null;
            }

            return new Message(messageEventDTO, null, _client);
        }

        public IMessage CreateMessage(IMessageEventDTO messageEventDTO, IApp app)
        {
            if (messageEventDTO == null)
            {
                return null;
            }

            return new Message(messageEventDTO, app, _client);
        }

        public IMessage CreateMessage(string json)
        {
            var eventWithAppDTO = _jsonObjectConverter.Deserialize<IMessageEventDTO>(json);
            return CreateMessage(eventWithAppDTO);
        }

        public IMessage CreateMessageFromMessageEventWithApp(string json)
        {
            var eventWithAppDTO = _jsonObjectConverter.Deserialize<IMessageEventWithAppDTO>(json);

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

            return new Message(messageEventDTO, app, _client);
        }

        public IRelationshipState CreateRelationshipState(string json)
        {
            var dto = _jsonObjectConverter.Deserialize<IRelationshipStateDTO>(json);
            return CreateRelationshipState(dto);
        }

        public IRelationshipState CreateRelationshipState(IRelationshipStateDTO relationshipStateDTO)
        {
            return relationshipStateDTO == null ? null : new RelationshipState(relationshipStateDTO);
        }

        public IRelationshipState[] CreateRelationshipStates(IRelationshipStateDTO[] relationshipStateDTOs)
        {
            if (relationshipStateDTOs == null)
            {
                return new IRelationshipState[0];
            }

            return relationshipStateDTOs?.Select(dto => _client.Factories.CreateRelationshipState(dto)).ToArray();
        }

        public IRelationshipDetails CreateRelationshipDetails(string json)
        {
            var dto = _jsonObjectConverter.Deserialize<IRelationshipDetailsDTO>(json);
            return CreateRelationshipDetails(dto);
        }

        public IRelationshipDetails CreateRelationshipDetails(IRelationshipDetailsDTO dto)
        {
            return dto == null ? null : new RelationshipDetails(dto);
        }

        public ISavedSearch CreateSavedSearch(string json)
        {
            var dto = _jsonObjectConverter.Deserialize<ISavedSearchDTO>(json);
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
            var tweetDTO = _jsonObjectConverter.Deserialize<ITweetDTO>(json);
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

        public ITweet[] CreateTweets(ITweetDTO[] tweetDTOs)
        {
            return tweetDTOs?.Select(x => CreateTweet(x)).ToArray();
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
            var dto = _jsonObjectConverter.Deserialize<IOEmbedTweetDTO>(json);
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
            var tweetDTO = _jsonObjectConverter.Deserialize<IUserDTO>(json);
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
            var tweetDTO = _jsonObjectConverter.Deserialize<IUserDTO>(json);
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

        public IAccountSettings CreateAccountSettings(string json)
        {
            var accountSettingsDTO = _jsonObjectConverter.Deserialize<IAccountSettingsDTO>(json);

            if (accountSettingsDTO == null)
            {
                return null;
            }

            return new AccountSettings(accountSettingsDTO);
        }
    }
}