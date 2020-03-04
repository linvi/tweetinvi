using System.Collections.Generic;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Events;

namespace Tweetinvi.Client.Tools
{
    public interface ITwitterClientFactories
    {
        // LISTS
        ITwitterList CreateTwitterList(string json);
        ITwitterList CreateTwitterList(ITwitterListDTO twitterListDTO);
        ITwitterList[] CreateTwitterLists(IEnumerable<ITwitterListDTO> listDTOs);

        // MESSAGE
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
        IRelationshipDetails CreateRelationshipDetails(string json);
        IRelationshipDetails CreateRelationshipDetails(IRelationshipDetailsDTO dto);

        // SAVED SEARCH
        ISavedSearch CreateSavedSearch(string json);
        ISavedSearch CreateSavedSearch(ISavedSearchDTO savedSearchDTO);

        // SEARCH
        ISearchResult CreateSearchResult(ISearchResultsDTO[] searchResultsDTO);
        ISearchQueryResult CreateSearchQueryResult(ISearchResultsDTO searchResultsDTO);

        // TWEET
        ITweet CreateTweet(string json);
        ITweet CreateTweet(ITweetDTO tweetDTO);
        ITweetWithSearchMetadata CreateTweetWithSearchMetadata(ITweetWithSearchMetadataDTO tweetDTO);
        IOEmbedTweet CreateOEmbedTweet(string json);
        IOEmbedTweet CreateOEmbedTweet(IOEmbedTweetDTO oEmbedTweetDTO);

        // USER
        IUser CreateUser(string json);
        IUser CreateUser(IUserDTO userDTO);
        IUser[] CreateUsers(IEnumerable<IUserDTO> usersDTO);
        IAuthenticatedUser CreateAuthenticatedUser(string json);
        IAuthenticatedUser CreateAuthenticatedUser(IUserDTO userDTO);
        IAccountSettings GenerateAccountSettingsFromJson(string json);
    }
}