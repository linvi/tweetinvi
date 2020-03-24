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
        IAccountSettings CreateAccountSettings(string json);
        IAccountSettings CreateAccountSettings(IAccountSettingsDTO dto);

        // LISTS
        ITwitterList CreateTwitterList(string json);
        ITwitterList CreateTwitterList(ITwitterListDTO twitterListDTO);
        ITwitterList[] CreateTwitterLists(IEnumerable<ITwitterListDTO> listDTOs);

        // MESSAGE
        IMessage CreateMessageFromMessageEventWithApp(string json);
        IMessage CreateMessage(string json);

        IMessage CreateMessage(IMessageEventDTO messageEventDTO);
        IMessage CreateMessage(IMessageEventDTO messageEventDTO, IApp app);
        IMessage CreateMessage(IGetMessageDTO getMessageDTO);
        IMessage CreateMessage(ICreateMessageDTO createMessageDTO);
        IMessage CreateMessage(IMessageEventWithAppDTO messageEventWithAppDTO);
        IMessage[] CreateMessages(IEnumerable<IMessageEventWithAppDTO> eventWithAppDTOs);

        // RELATIONSHIP
        IRelationshipState CreateRelationshipState(string json);
        IRelationshipState CreateRelationshipState(IRelationshipStateDTO relationshipStateDTO);
        IRelationshipState[] CreateRelationshipStates(IRelationshipStateDTO[] relationshipStateDTOs);
        IRelationshipDetails CreateRelationshipDetails(string json);
        IRelationshipDetails CreateRelationshipDetails(IRelationshipDetailsDTO dto);

        // SAVED SEARCH
        ISavedSearch CreateSavedSearch(string json);
        ISavedSearch CreateSavedSearch(ISavedSearchDTO savedSearchDTO);

        // SEARCH
        ISearchResults CreateSearchResult(ISearchResultsDTO searchResultsDTO);

        // TWEET
        ITweet CreateTweet(string json);
        ITweet CreateTweet(ITweetDTO tweetDTO);
        ITweet[] CreateTweets(IEnumerable<ITweetDTO> tweetDTOs);
        ITweetWithSearchMetadata CreateTweetWithSearchMetadata(ITweetWithSearchMetadataDTO tweetWithSearchMetadataDTO);
        IOEmbedTweet CreateOEmbedTweet(string json);
        IOEmbedTweet CreateOEmbedTweet(IOEmbedTweetDTO oEmbedTweetDTO);

        // USER
        IUser CreateUser(string json);
        IUser CreateUser(IUserDTO userDTO);
        IUser[] CreateUsers(IEnumerable<IUserDTO> usersDTO);
        IAuthenticatedUser CreateAuthenticatedUser(string json);
        IAuthenticatedUser CreateAuthenticatedUser(IUserDTO userDTO);

        // WEBHOOKS
        IWebhook CreateWebhook(string json);
        IWebhook CreateWebhook(IWebhookDTO webhookDTO);

        IWebhookEnvironment CreateWebhookEnvironment(string json);
        IWebhookEnvironment CreateWebhookEnvironment(IWebhookEnvironmentDTO webhookEnvironmentDTO);

        IWebhookEnvironmentSubscriptions CreateWebhookEnvironmentSubscriptions(string json);
        IWebhookEnvironmentSubscriptions CreateWebhookEnvironmentSubscriptions(IWebhookEnvironmentSubscriptionsDTO webhookEnvironmentSubscriptionsDTO);

        // HELP
        ITwitterConfiguration CreateTwitterConfiguration(string json);

        // RATE LIMITS
        ICredentialsRateLimits CreateRateLimits(string json);
        ICredentialsRateLimits CreateRateLimits(CredentialsRateLimitsDTO dto);

        // CREDENTIALS
        ITwitterCredentials CreateTwitterCredentials(string json);
        IConsumerCredentials CreateConsumerCredentials(string json);

        // MEDIA
        IMedia CreateMedia(string json);
        IUploadedMediaInfo CreateUploadedMediaInfo(string json);

        // SEARCH
        ISearchResults CreateSearchResult(string json);
    }
}