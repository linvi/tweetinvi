using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Models
{
    /// <summary>
    /// A token user is unique to a Token and provides action that will
    /// be executed from the connected user and that are not available
    /// from another user like (read my messages)
    /// </summary>
    public class AuthenticatedUser : User, IAuthenticatedUser
    {
        private readonly IMessageController _messageController;
        private readonly ISavedSearchController _savedSearchController;

        public AuthenticatedUser(IUserDTO userDTO, ITwitterClient client) : base(userDTO, client)
        {
            var executionContext = client.CreateTwitterExecutionContext();
            _messageController = executionContext.Container.Resolve<IMessageController>();
            _savedSearchController = executionContext.Container.Resolve<ISavedSearchController>();
        }

        public string Email => UserDTO.Email;

        public IReadOnlyTwitterCredentials Credentials => Client.Credentials;

        // Home Timeline
        public ITwitterIterator<ITweet, long?> GetHomeTimelineIterator()
        {
            return Client.Timelines.GetHomeTimelineIterator();
        }

        public ITwitterIterator<ITweet, long?> GetHomeTimelineIterator(IGetHomeTimelineParameters parameters)
        {
            return Client.Timelines.GetHomeTimelineIterator(parameters);
        }

        public ITwitterIterator<ITweet, long?> GetMentionsTimelineIterator()
        {
            return Client.Timelines.GetMentionsTimelineIterator();
        }

        public ITwitterIterator<ITweet, long?> GetMentionsTimelineIterator(IGetMentionsTimelineParameters parameters)
        {
            return Client.Timelines.GetMentionsTimelineIterator();
        }

        // Friendships
        public Task UpdateRelationship(IUpdateRelationshipParameters parameters)
        {
            return Client.Account.UpdateRelationship(parameters);
        }

        // Friends - Followers
        public ITwitterIterator<long> GetUserIdsRequestingFriendship()
        {
            return Client.Account.GetUserIdsRequestingFriendship(new GetUserIdsRequestingFriendshipParameters());
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship()
        {
            return Client.Account.GetUsersRequestingFriendship(new GetUsersRequestingFriendshipParameters());
        }

        public ITwitterIterator<long> GetUserIdsYouRequestedToFollow()
        {
            return Client.Account.GetUserIdsYouRequestedToFollow();
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollow()
        {
            return Client.Account.GetUsersYouRequestedToFollow(new GetUsersYouRequestedToFollowParameters());
        }


        // Follow
        public Task FollowUser(IUserIdentifier user)
        {
            return Client.Account.FollowUser(user);
        }

        public Task FollowUser(long userId)
        {
            return Client.Account.FollowUser(userId);
        }

        public Task FollowUser(string username)
        {
            return Client.Account.FollowUser(username);
        }

        public Task UnFollowUser(IUserIdentifier user)
        {
            return Client.Account.UnFollowUser(user);
        }

        public Task UnFollowUser(long userId)
        {
            return Client.Account.UnFollowUser(userId);
        }

        public Task UnFollowUser(string username)
        {
            return Client.Account.UnFollowUser(username);
        }

        public Task<IEnumerable<ISavedSearch>> GetSavedSearches()
        {
            return _savedSearchController.GetSavedSearches();
        }

        // Block
        public override Task BlockUser()
        {
            throw new InvalidOperationException("You cannot block yourself...");
        }

        public Task BlockUser(IUserIdentifier user)
        {
            return Client.Account.BlockUser(user);
        }

        public Task BlockUser(long? userId)
        {
            return Client.Account.BlockUser(userId);
        }

        public Task BlockUser(string username)
        {
            return Client.Account.BlockUser(username);
        }

        // Unblock
        public override Task UnBlockUser()
        {
            throw new InvalidOperationException("You cannot unblock yourself...");
        }

        public Task UnBlockUser(IUserIdentifier user)
        {
            return Client.Account.UnBlockUser(user);
        }

        public Task UnBlockUser(long userId)
        {
            return Client.Account.UnblockUser(userId);
        }

        public Task UnBlockUser(string username)
        {
            return Client.Account.UnblockUser(username);
        }

        // Get Blocked Users
        public ITwitterIterator<long> GetBlockedUserIds()
        {
            return Client.Account.GetBlockedUserIds();
        }

        public ITwitterIterator<IUser> GetBlockedUsers()
        {
            return Client.Account.GetBlockedUsers();
        }

        // Spam
        public override Task ReportUserForSpam()
        {
            throw new InvalidOperationException("You cannot report yourself for spam...");
        }

        public Task ReportUserForSpam(IUserIdentifier user)
        {
            return Client.Account.ReportUserForSpam(user);
        }

        public Task ReportUserForSpam(string username)
        {
            return Client.Account.ReportUserForSpam(username);
        }

        public Task ReportUserForSpam(long? userId)
        {
            return Client.Account.ReportUserForSpam(userId);
        }

        // Direct Messages
        public async Task<IEnumerable<IMessage>> GetLatestMessages(int count)
        {
            var asyncOperation = await GetLatestMessagesWithCursor(count);
            return asyncOperation.Result;
        }

        public Task<AsyncCursorResult<IEnumerable<IMessage>>> GetLatestMessagesWithCursor(int count)
        {
            return _messageController.GetLatestMessages(count);
        }

        public Task<IMessage> PublishMessage(IPublishMessageParameters publishMessageParameters)
        {
            return _messageController.PublishMessage(publishMessageParameters);
        }

        // Tweet
        public Task<ITweet> PublishTweet(IPublishTweetParameters parameters)
        {
            return Client.Tweets.PublishTweet(parameters);
        }

        public Task<ITweet> PublishTweet(string text)
        {
            return Client.Tweets.PublishTweet(text);
        }

        // Settings
        public Task<IAccountSettings> UpdateAccountSettings(IUpdateAccountSettingsParameters parameters)
        {
            return Client.AccountSettings.UpdateAccountSettings(parameters);
        }

        // Twitter Lists
        public Task<ITwitterList> SubscribeToList(ITwitterListIdentifier list)
        {
            return Client.Lists.SubscribeToList(list);
        }

        public Task<ITwitterList> SubscribeToList(long? listId)
        {
            return Client.Lists.SubscribeToList(listId);
        }

        public Task<ITwitterList> UnSubscribeFromList(ITwitterListIdentifier list)
        {
            return Client.Lists.UnsubscribeFromList(list);
        }

        public Task<ITwitterList> UnSubscribeFromList(long? listId)
        {
            return Client.Lists.UnsubscribeFromList(listId);
        }

        // Mute
        public ITwitterIterator<long> GetMutedUserIds()
        {
            return Client.Account.GetMutedUserIds();
        }

        public ITwitterIterator<IUser> GetMutedUsers()
        {
            return Client.Account.GetMutedUsers();
        }

        public Task MuteUser(IUserIdentifier user)
        {
            return Client.Account.MuteUser(user);
        }

        public Task MuteUser(long? userId)
        {
            return Client.Account.MuteUser(userId);
        }

        public Task MuteUser(string username)
        {
            return Client.Account.MuteUser(username);
        }

        public Task UnMuteUser(IUserIdentifier user)
        {
            return Client.Account.UnMuteUser(user);
        }

        public Task UnMuteUser(long userId)
        {
            return Client.Account.UnMuteUser(userId);
        }

        public Task UnMuteUser(string username)
        {
            return Client.Account.UnMuteUser(username);
        }
    }
}