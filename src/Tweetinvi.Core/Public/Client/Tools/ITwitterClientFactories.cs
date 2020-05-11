using System.Collections.Generic;
using Tweetinvi.Core.DTO;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Events;
using Tweetinvi.Models.DTO.Webhooks;

namespace Tweetinvi.Client.Tools
{
    public interface ITwitterClientFactories
    {
        // ACCOUNT SETTINGS

        /// <summary>
        /// Creates accountSettings from json
        /// </summary>
        IAccountSettings CreateAccountSettings(string json);
        IAccountSettings CreateAccountSettings(IAccountSettingsDTO dto);

        // LISTS

        /// <summary>
        /// Create TwitterList from json
        /// </summary>
        ITwitterList CreateTwitterList(string json);
        ITwitterList CreateTwitterList(ITwitterListDTO twitterListDTO);
        ITwitterList[] CreateTwitterLists(IEnumerable<ITwitterListDTO> listDTOs);

        // MESSAGE

        /// <summary>
        /// Creates a message from create event message json
        /// </summary>
        IMessage CreateMessageFromMessageEventWithApp(string json);

        /// <summary>
        /// Creates a Message from json
        /// </summary>
        IMessage CreateMessage(string json);

        IMessage CreateMessage(IMessageEventDTO messageEventDTO);
        IMessage CreateMessage(IMessageEventDTO messageEventDTO, IApp app);
        IMessage CreateMessage(IGetMessageDTO getMessageDTO);
        IMessage CreateMessage(ICreateMessageDTO createMessageDTO);
        IMessage CreateMessage(IMessageEventWithAppDTO messageEventWithAppDTO);
        IMessage[] CreateMessages(IEnumerable<IMessageEventWithAppDTO> eventWithAppDTOs);

        // RELATIONSHIP

        /// <summary>
        /// Creates a relationship state from json
        /// </summary>
        IRelationshipState CreateRelationshipState(string json);
        IRelationshipState CreateRelationshipState(IRelationshipStateDTO relationshipStateDTO);
        IRelationshipState[] CreateRelationshipStates(IRelationshipStateDTO[] relationshipStateDTOs);
        IRelationshipDetails CreateRelationshipDetails(string json);
        IRelationshipDetails CreateRelationshipDetails(IRelationshipDetailsDTO dto);

        // SAVED SEARCH

        /// <summary>
        /// Creates a saved search from json
        /// </summary>
        ISavedSearch CreateSavedSearch(string json);
        ISavedSearch CreateSavedSearch(ISavedSearchDTO savedSearchDTO);

        // SEARCH
        ISearchResults CreateSearchResult(ISearchResultsDTO searchResultsDTO);

        // TWEET

        /// <summary>
        /// Creates a tweet from json
        /// </summary>
        ITweet CreateTweet(string json);
        ITweet CreateTweet(ITweetDTO tweetDTO);
        ITweet[] CreateTweets(IEnumerable<ITweetDTO> tweetDTOs);
        ITweetWithSearchMetadata CreateTweetWithSearchMetadata(ITweetWithSearchMetadataDTO tweetWithSearchMetadataDTO);

        /// <summary>
        /// Creates a oembed tweet from json
        /// </summary>
        IOEmbedTweet CreateOEmbedTweet(string json);
        IOEmbedTweet CreateOEmbedTweet(IOEmbedTweetDTO oEmbedTweetDTO);

        // USER

        /// <summary>
        /// Creates a user from json
        /// </summary>
        IUser CreateUser(string json);
        IUser CreateUser(IUserDTO userDTO);
        IUser[] CreateUsers(IEnumerable<IUserDTO> usersDTO);

        /// <summary>
        /// Creates an authenticated user from json
        /// </summary>
        IAuthenticatedUser CreateAuthenticatedUser(string json);
        IAuthenticatedUser CreateAuthenticatedUser(IUserDTO userDTO);

        // WEBHOOKS

        /// <summary>
        /// Creates a webhook from json
        /// </summary>
        IWebhook CreateWebhook(string json);
        IWebhook CreateWebhook(IWebhookDTO webhookDTO);

        /// <summary>
        /// Creates a webhook environment from json
        /// </summary>
        IWebhookEnvironment CreateWebhookEnvironment(string json);
        IWebhookEnvironment CreateWebhookEnvironment(IWebhookEnvironmentDTO webhookEnvironmentDTO);

        /// <summary>
        /// Creates a webhook subscription from json
        /// </summary>
        IWebhookEnvironmentSubscriptions CreateWebhookEnvironmentSubscriptions(string json);
        IWebhookEnvironmentSubscriptions CreateWebhookEnvironmentSubscriptions(IWebhookEnvironmentSubscriptionsDTO webhookEnvironmentSubscriptionsDTO);

        // HELP

        /// <summary>
        /// Creates a twitter configuration from json
        /// </summary>
        ITwitterConfiguration CreateTwitterConfiguration(string json);

        // RATE LIMITS

        /// <summary>
        /// Creates a RateLimits object from json
        /// </summary>
        ICredentialsRateLimits CreateRateLimits(string json);
        ICredentialsRateLimits CreateRateLimits(CredentialsRateLimitsDTO dto);

        // CREDENTIALS

        /// <summary>
        /// Creates credentials from json
        /// </summary>
        ITwitterCredentials CreateTwitterCredentials(string json);

        /// <summary>
        /// Creates consumer credentials from json
        /// </summary>
        IConsumerCredentials CreateConsumerCredentials(string json);

        // MEDIA

        /// <summary>
        /// Creates a media from json
        /// </summary>
        IMedia CreateMedia(string json);

        /// <summary>
        /// Creates uploaded media information from json
        /// </summary>
        IUploadedMediaInfo CreateUploadedMediaInfo(string json);

        // SEARCH

        /// <summary>
        /// Creates search results from json
        /// </summary>
        ISearchResults CreateSearchResult(string json);
    }
}